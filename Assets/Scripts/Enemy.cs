using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
	public int playerDamage;

	private Animator animator;
	private Transform target;
	private bool skipMove;

    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    public EnemyTarget enemyTargetPrefab;
    public EnemyMove enemyMovePrefab;

    private Vector2 moveLocation;
    private Vector2 attackLocation;

    public Sprite arrowRight;
    public Sprite arrowDown;
    public Sprite arrowLeft;
    public Sprite arrowUp;

    public EnemyIntent enemyIntent;

	public GameObject enemyAttackLocationPrefab;
	public List<GameObject> enemyAttackLocations = new List<GameObject>();

    enum Direction
    {
        L,
        R,
        U,
        D
    }

    public void Awake()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    private Vector2 DirectionToCoordinates(Direction d)
    {
        switch (d)
        {
            case Direction.L:
                return new Vector2(-1, 0);
            case Direction.R:
                return new Vector2(1, 0);
            case Direction.U:
                return new Vector2(0, 1);
            case Direction.D:
                return new Vector2(0, -1);
            default:
                throw new System.Exception("unknown direction: " + d);
        }
    }

    private Quaternion GetRotationFromDirection(Direction d)
    {
        switch (d)
        {
            case Direction.R:
                return Quaternion.identity;
            case Direction.D:
                return Quaternion.Euler(0, 90f, 0);
            case Direction.L:
                return Quaternion.Euler(0, 180f, 0);
            case Direction.U:
                return Quaternion.Euler(0, 270f, 0);
            default:
                throw new System.Exception("Didn't recognize direction: " + d);
        }
    }

    private Sprite GetSpriteFromDirection(Direction d)
    {
        switch (d)
        {
            case Direction.R:
                return arrowRight;
            case Direction.D:
                return arrowDown;
            case Direction.L:
                return arrowLeft;
            case Direction.U:
                return arrowUp;
            default:
                throw new System.Exception("Didn't recognize direction: " + d);
        }
    }

    protected EnemyIntent GenerateIntent()
    {
        if (GameManager.instance.turn % 2 == 0)
        {
            // Attack
            Direction direction = GenEnemyTargetDirection();
            Vector2 start = (Vector2) transform.position;
            Vector2 location = start + DirectionToCoordinates(direction);

            EnemyTarget enemyTarget = Instantiate(enemyTargetPrefab, location, Quaternion.identity);
            return new AttackLocation(location);

        } else
        {
            // Move
            Direction direction;

            if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
                direction = target.position.y > transform.position.y ? Direction.U : Direction.D;
            else
                direction = target.position.x > transform.position.x ? Direction.R : Direction.L;

            Vector2 start = (Vector2)transform.position;
            Vector2 location = start + DirectionToCoordinates(direction);

            EnemyMove enemyMove = Instantiate(enemyMovePrefab, transform.position, Quaternion.identity);
            SpriteRenderer spriteRenderer = enemyMove.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = GetSpriteFromDirection(direction);
           
            return new MoveToLocation(location);
        }
    }

    public void Intend()
    {
        enemyIntent = GenerateIntent();
    }

    //MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
    public void MoveEnemy()
    {
        switch (enemyIntent)
        {
            case MoveToLocation moveToLocation:
                Transform hitTransform = CanMove(moveToLocation.location);
                if (!hitTransform)
                {
                    MoveSmooth(moveToLocation.location);
                }
                return;
            case AttackLocation attackLocation:
                animator.SetTrigger("enemyAttack");
                SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);

				GameObject enemyAttackLocation = Instantiate(enemyAttackLocationPrefab, (Vector3)attackLocation.location, Quaternion.identity);
				enemyAttackLocation.GetComponent<EnemyAttackLocation>().enemy = this;
				enemyAttackLocations.Add(enemyAttackLocation);

                return;
            default:
                throw new System.Exception("Not sure what to do with intent " + enemyIntent);

        }
    }

    public void DestroyAttacks()
	{
        foreach (GameObject enemyAttackLocation in enemyAttackLocations)
		{
			Destroy(enemyAttackLocation);
		}
	}

    private Direction GenEnemyTargetDirection()
    {
        Direction[] directions = { Direction.U, Direction.D, Direction.L, Direction.R };
        return directions[Random.Range(0, directions.Length)];
    }
}

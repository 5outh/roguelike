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

            // NB. This is just visual right now
            EnemyTarget enemyTarget = Instantiate(enemyTargetPrefab, location, Quaternion.identity);
            return new AttackLocation(start + location);

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

            // NB. This is just visual right now
            EnemyMove enemyMove = Instantiate(enemyMovePrefab, transform.position, Quaternion.identity);
            SpriteRenderer spriteRenderer = enemyMove.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = GetSpriteFromDirection(direction);
           
            return new MoveToLocation(location);
        }
    }

    public void Intend()
    {
        print("Intend was called");
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

                // TODO: Spawn EnemyAttackLocation and check for player collision.
                return;
            default:
                throw new System.Exception("Not sure what to do with intent " + enemyIntent);

        }
        /**
         * Desired behavior for enemy ai:
         *
         * Before the Player's turn, generate an *Intent*
         * An Intent can be either:
         *
         * - Attack to the left or right
         * - Move to the left or right
         *
         * Player executes their turn.
         *
         * On the enemy's turn:
         *
         * - If intent is to attack, check if intended attack location collides with player.
         *   - If so, hurt the player for playerDamage amount of food
         * - If the intent is to move, move the enemy in the intended direction.
         */

        //int xDir = 0;
        //int yDir = 0;

        //// TODO: Figure out a position earlier?
        //if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        //    yDir = target.position.y > transform.position.y ? 1 : -1;
        //else
        //    xDir = target.position.x > transform.position.x ? 1 : -1;

        //Vector2 end = GetMoveCoordinates(xDir, yDir);
        //Transform hitTransform = CanMove(end);

        //if (hitTransform == null)
        //{
        //    if (skipMove)
        //    {
        //        Debug.Log("Generating enemy target");
        //        GenEnemyTarget();
        //        skipMove = false;
        //        return;
        //    }

        //    GenEnemyAttackRight();

        //    MoveSmooth(end);
        //    skipMove = true;
        //} else
        //{
        //    Player hitPlayer = hitTransform.GetComponent<Player>();
        //    if (hitPlayer != null) {
        //        animator.SetTrigger("enemyAttack");
        //        hitPlayer.LoseFood(playerDamage);
        //        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
        //    }
        //}
    }

    private Direction GenEnemyTargetDirection()
    {
        Direction[] directions = { Direction.U, Direction.D, Direction.L, Direction.R };
        return directions[Random.Range(0, directions.Length)];
    }
}

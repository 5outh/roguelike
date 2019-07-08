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
    public GameObject enemyAttackRightPrefab;

    private Vector2 moveLocation;
    private Vector2 attackLocation;

    private enum Intent
    {
        Move,
        Attack
    }

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        base.Start();
    }

    //MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
    public void MoveEnemy()
    {
        // TODO: This AI is sort of stupid.
        // Can we control it elsewhere? Split an EnemyController out of
        // the GameManager?

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

        int xDir = 0;
        int yDir = 0;

        // TODO: Figure out a position earlier?
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        Vector2 end = GetMoveCoordinates(xDir, yDir);
        Transform hitTransform = CanMove(end);

        if (hitTransform == null)
        {
            if (skipMove)
            {
                Debug.Log("Generating enemy target");
                GenEnemyTarget();
                skipMove = false;
                return;
            }

            GenEnemyAttackRight();

            MoveSmooth(end);
            skipMove = true;
        } else
        {
            Player hitPlayer = hitTransform.GetComponent<Player>();
            if (hitPlayer != null) {
                animator.SetTrigger("enemyAttack");
                hitPlayer.LoseFood(playerDamage);
                SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
            }
        }
    }

    public EnemyTarget GenEnemyTarget()
    {
        Vector2 position = GenEnemyTargetPosition();
        EnemyTarget enemyTarget = Instantiate(enemyTargetPrefab, position, Quaternion.identity);
        return enemyTarget;
    }


    public GameObject GenEnemyAttackRight()
    {
        // TODO: Get the correct rotation for the object
        Quaternion rotation = Quaternion.identity;

        GameObject enemyAttackRight = Instantiate(enemyAttackRightPrefab, transform.position, rotation);
        return enemyAttackRight;
    }

    public Vector2 GenEnemyTargetPosition()
    {
        int[] possibilities = { 1, -1 };
        Vector2 start = transform.position;

        int addX = possibilities[Random.Range(0, possibilities.Length)];
        Debug.Log(addX);

        return start + new Vector2((float)addX, 0);
    }
}

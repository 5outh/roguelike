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
        int xDir = 0;
        int yDir = 0;

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
                skipMove = false;
                return;
            }

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
}

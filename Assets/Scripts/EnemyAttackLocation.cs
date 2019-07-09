﻿using UnityEngine;

public class EnemyAttackLocation : MonoBehaviour
{
    public Enemy enemy;

    private void OnDrawGizmos()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        Gizmos.DrawWireCube(transform.position, boxCollider2D.size);
    }

    // Check if this location is currently colliding with the player.
    public bool IsCollidingWithPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        CollisionList collisionList = gameObject.GetComponent<CollisionList>();

        return collisionList.Get(playerObject.GetInstanceID()) != null;
    }

    public void AttackPlayerIfColliding(int attackDamage)
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Player player = playerObject.GetComponent<Player>();

        if (IsCollidingWithPlayer())
        {
            player.LoseFood(attackDamage);
        }
    }
}
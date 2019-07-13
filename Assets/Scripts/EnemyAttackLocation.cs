using UnityEngine;

public class EnemyAttackLocation : MonoBehaviour
{
    public Enemy enemy;

    private void OnDrawGizmos()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        Gizmos.DrawWireCube(transform.position, boxCollider2D.size);
    }
}
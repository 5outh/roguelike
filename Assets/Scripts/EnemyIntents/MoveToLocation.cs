using UnityEngine;

/**
 * Move to a location on the board.
 */
public class MoveToLocation : EnemyIntent
{
    public Vector2 location;

    public MoveToLocation(Vector2 location)
    {
        this.location = location;
    }
}
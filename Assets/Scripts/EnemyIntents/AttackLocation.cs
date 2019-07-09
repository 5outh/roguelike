using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Attack a given location on the board.
 */
public class AttackLocation : EnemyIntent
{
    public Vector2 location;

    public AttackLocation(Vector2 location)
    {
        this.location = location;
    }
}

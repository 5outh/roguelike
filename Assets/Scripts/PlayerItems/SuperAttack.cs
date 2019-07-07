using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperAttack : PlayerItem
{
    public override void OnPlayerTurnStart()
    {
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();

        if (isActive)
        {
            spriteRenderer.color = Color.red;
        } else
        {
            // Not sure why this has to be here, but things need to reset
            // at the beginning of the turn.
            // I think there is a race condition on Update functions.
            //spriteRenderer.color = Color.white;
        }
    }

    public override void OnPlayerTurnEnd()
    {
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
    }

    public override void OnHitEnemy(Enemy enemy)
    {
        if (isActive)
        {
            GameManager.instance.KillEnemy(enemy);
        }
    }
}

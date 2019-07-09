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
            player.GetComponent<Animator>().SetTrigger("playerChop");
            GameManager.instance.KillEnemy(enemy);
        }
    }
}

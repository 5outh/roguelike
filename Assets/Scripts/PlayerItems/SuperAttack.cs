using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperAttack : PlayerItem
{
    public override void Display()
    {
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();

        if (isActive)
        {
            spriteRenderer.color = Color.red;
            return;
        }

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

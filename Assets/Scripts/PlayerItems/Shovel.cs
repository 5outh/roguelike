using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : PlayerItem
{
    public override void OnHitWall(Wall wall)
    {
        if (isActive)
        {
            wall.DamageWall(player.wallDamage);
            player.GetComponent<Animator>().SetTrigger("playerChop");
        }
    }
}
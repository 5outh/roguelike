using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : PlayerItem
{
    public override void OnPlayerTurnStart()
    {
        GameObject shield = player.shield;

        if (isActive)
        {
            shield.transform.position = player.transform.position;
            shield.SetActive(true);
            player.damageNegated = true;
        }
    }

    public override void OnLoseFood(int loss)
    {
        if (isActive)
        {
            player.damageNegated = true;
        }
    }

    public override void OnPlayerTurnEnd()
    {
        GameObject shield = player.shield;
        shield.SetActive(false);
        player.damageNegated = false;
    }
}

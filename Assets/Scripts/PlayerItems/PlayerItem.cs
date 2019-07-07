using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerItem : MonoBehaviour
{
    // The Player this is attached to.
    public Player player;

    // Is the item currently enabled?
    public bool isActive;

    // Current turn
    public int turn = 1;

    // Cycle length of this item
    public int cycleLength;

    // The icon to show in the item cycle
    public Image icon;

    protected virtual void Activate()
    {
        isActive = true;
    }

    protected virtual void Deactivate()
    {
        isActive = false;
    }

    public virtual void OnHitEnemy(Enemy enemy)
    {
        // By default, do nothing
    }

    public virtual void OnHitWall(Wall wall)
    {
        // By default, do nothing
    }

    public virtual void OnLoseFood(int loss)
    {
        // By default, do nothing
    }

    public virtual void Display()
    {
        // by default, do nothing
    }

    public virtual void OnPlayerTurnStart()
    {
        // by default, do nothing
    }

    public virtual void OnPlayerTurnEnd()
    {
        // by default, do nothing
    }

    public void UpdateTurn(int theTurn)
    {
        turn = theTurn;

        if (turn % cycleLength == 0)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    // Check if this item is active on the current turn
    public bool isActiveOnTurn(int theTurn)
    {
        return theTurn % cycleLength == 0;
    }
}
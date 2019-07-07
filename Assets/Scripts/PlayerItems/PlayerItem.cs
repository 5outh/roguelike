using UnityEngine;

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

        Debug.Log("Turn: " + turn);
        Debug.Log("Cycle Length: " + cycleLength);

        if (turn % cycleLength == 0)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }
}
using UnityEngine;

public abstract class PlayerItem : MonoBehaviour
{
    // The Player this is attached to.
    public Player player;

    // Is the item currently enabled?
    public bool isActive;

    // Current turn
    [HideInInspector] public int turn;

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

    public virtual void OnLoseFood()
    {
        // By default, do nothing
    }

    public virtual void Display()
    {
        // by default, do nothing
    }

    private void Update()
    {
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
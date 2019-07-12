using UnityEngine;

public class Exit : MonoBehaviour
{
    public bool open = false;
    public Sprite openDoor;
    public Sprite closedDoor;

    public void Open()
    {
        open = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = openDoor;
        BoxCollider2D boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
    }
}

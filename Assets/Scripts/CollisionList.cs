using UnityEngine;
using System.Collections.Generic;

/**
 * Track GameObjects that are currently colliding with the Entity in question via a Trigger.
 */
public class CollisionList : MonoBehaviour
{
    private Dictionary<int, GameObject> collisionDict = new Dictionary<int, GameObject>();

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Add " + col.gameObject);
        collisionDict.Add(col.gameObject.GetInstanceID(), gameObject);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("Remove " + col.gameObject);
        collisionDict.Remove(col.gameObject.GetInstanceID());
    }

    // Get the game object at a given key.
    public GameObject Get(int key)
    {
        try
        {
            return collisionDict[key];
        } catch (KeyNotFoundException)
        {
            // Doesn't matter.
            return null;
        }
    }
}
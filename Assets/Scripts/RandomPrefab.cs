using UnityEngine;

// Replace this GameObject with a random prefab from a specified list.
public class RandomPrefab : MonoBehaviour
{
    public GameObject[] prefabs;

    GameObject GetRandomPrefab()
    {
        return prefabs[Random.Range(0, prefabs.Length)];
    }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(GetRandomPrefab(), transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);
    }
}
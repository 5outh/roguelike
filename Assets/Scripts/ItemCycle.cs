using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCycle : MonoBehaviour
{
    public GameObject itemCycleElementPrefab;
    public List<GameObject> itemCycleElements;

    public int turn;

    public void Render(List<PlayerItem> playerItems, int theTurn)
    {
        //Only render once per frame.
        if (theTurn == turn) return;

        turn = theTurn;

        foreach (GameObject item in itemCycleElements)
        {
            Destroy(item);
        }

        itemCycleElements = GetItemCycleElements(playerItems, turn);
    }

    public List<GameObject> GetItemCycleElements(
        List<PlayerItem> playerItems,
        int turn
    ) {
        List<GameObject> elements = new List<GameObject>();

        // Traverse in reverse to populate most recent
        // item to the bottom of this list
        for (int i = 7; i >= 0; i--)
        {
            // Instantiate a new row.
            GameObject itemCycleElement = Instantiate(
                itemCycleElementPrefab,
                gameObject.transform
            );

            foreach (PlayerItem playerItem in playerItems)
            {
                // Add an icon to the item cycle element.
                if (playerItem.IsActiveOnTurn(turn + i))
                {
                    Instantiate(playerItem.icon, itemCycleElement.transform);
                }
            }

            // Current turn: tint the active row
            if (i == 0)
            {
                itemCycleElement.GetComponent<Image>().color = Color.blue;
            }

            elements.Add(itemCycleElement);
        }

        // Right now this doesn't really matter. It just sets everything directly.
        // Eventually we'll want to shift these.
        return elements;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class GhostManager : MonoBehaviour
{
    public InventoryManager inventory;
    public List<ItemData> allItems;

    public ItemData currentRequestedItem;

    public void SpawnGhost() // Ghost Spawner
    {
        if (allItems.Count == 0) return;

        currentRequestedItem = allItems[Random.Range(0, allItems.Count)];

        Debug.Log("Ghost wants: " + currentRequestedItem.itemName);
    }

    public void ClearGhost() // removes ghost after transaction
    {
        currentRequestedItem = null;
    }
}

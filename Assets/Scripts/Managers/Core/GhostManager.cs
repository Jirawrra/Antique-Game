using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

public class GhostManager : MonoBehaviour
{
    public InventoryManager inventory;
    public List<ItemData> allItems;

    [HideInInspector]
    public ItemData currentRequestedItem;

    public Image AntiqueImage; // Reference to the UI Image component for displaying the requested item

    void Awake()
    {
        if (allItems != null && allItems.Count > 0)
        {
            RequestRandomItem();
        }
        else
        {
            Debug.LogWarning("GhostManager: allItems list is null or empty! Assign it in the Inspector.");
        }
    }

    public void RequestRandomItem()
    {
        if (allItems == null || allItems.Count == 0) return;
        currentRequestedItem = allItems[Random.Range(0, allItems.Count)];  //selects random item from list of all items
        Debug.Log("Ghost wants: " + currentRequestedItem.itemName);

        AntiqueImage.sprite = currentRequestedItem.icon; // Update the UI image to show the requested item

    }

    public void ClearGhost() // removes ghost after transaction
    {
        currentRequestedItem = null;
    }
}

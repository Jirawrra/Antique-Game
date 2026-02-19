using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

public class InventoryManager : MonoBehaviour
{
    public List<ItemData> inventory = new List<ItemData>();

    public void AddItem (ItemData item) // add item
    {
        inventory.Add(item);
        Debug.Log("Added to inventory: " + item.itemName);
    }

    public void RemoveItem(ItemData item) // remove item
    {
        inventory.Remove(item);
        Debug.Log("Removed from inventory: " + item.itemName);
    }

    public bool HasItem(ItemData item) // checks if the item exists
    {
        return inventory.Contains(item);
    }
}
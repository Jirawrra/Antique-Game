using UnityEngine;
using System;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private Dictionary<ItemData, int> itemStocks = new Dictionary<ItemData, int>();

    private void Awake()
    {
        Instance = this;
    }

    // Add stock
    public void AddItem(ItemData item, int amount)
    {
        if (itemStocks.ContainsKey(item))
            itemStocks[item] += amount;
        else
            itemStocks[item] = amount;

        Debug.Log($"Added {amount}x {item.itemName}. Total: {itemStocks[item]}");
    }

    // Remove stock
    public bool RemoveItem(ItemData item, int amount)
    {
        if (!itemStocks.ContainsKey(item) || itemStocks[item] < amount)
            return false;

        itemStocks[item] -= amount;
        return true;
    }

    // Get stock
    public int GetStock(ItemData item)
    {
        return itemStocks.ContainsKey(item) ? itemStocks[item] : 0;
    }
}
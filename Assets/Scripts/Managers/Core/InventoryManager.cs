using UnityEngine;
using System;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<ItemData, int> itemStocks = new Dictionary<ItemData, int>();
    public event Action<ItemData, int> OnStockChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        // Subscribe safely
        if (TransactionManager.Instance != null)
            TransactionManager.Instance.OnItemDelivered += AddItem;
    }

    private void OnDisable()
    {
        if (TransactionManager.Instance != null)
            TransactionManager.Instance.OnItemDelivered -= AddItem;
    }

    /// <summary>
    /// Add stock for an item
    /// </summary>
    public void AddItem(ItemData item, int amount)
    {
        if (item == null || amount <= 0)
            return;

        if (itemStocks.ContainsKey(item))
            itemStocks[item] += amount;
        else
            itemStocks[item] = amount;

        Debug.Log($"Added {amount}x {item.itemName}. Total: {itemStocks[item]}");

        OnStockChanged?.Invoke(item, itemStocks[item]);
    }

    /// <summary>
    /// Remove stock for an item
    /// </summary>
    public bool RemoveItem(ItemData item, int amount)
    {
        if (item == null || amount <= 0)
            return false;

        if (!itemStocks.ContainsKey(item) || itemStocks[item] < amount)
            return false;

        itemStocks[item] -= amount;

        // Remove the item entirely if stock is zero
        if (itemStocks[item] <= 0)
            itemStocks.Remove(item);

        OnStockChanged?.Invoke(item, GetStock(item));

        return true;
    }

    /// <summary>
    /// Get current stock for an item
    /// </summary>
    public int GetStock(ItemData item)
    {
        if (item == null) return 0;
        return itemStocks.ContainsKey(item) ? itemStocks[item] : 0;
    }
}
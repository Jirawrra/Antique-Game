using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TransactionManager : MonoBehaviour
{
    public static TransactionManager Instance;

    // Events
    public event Action<ItemData, int> OnItemPurchased;   // Fired immediately after spending money
    public event Action<ItemData, int> OnItemDelivered;  // Fired after delivery delay

    private void Awake()
    {
        Instance = this;
    }

    // Represents a purchase in the queue
    private class PurchaseEntry
    {
        public ItemData item;
        public int amount;

        public PurchaseEntry(ItemData item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }

    // Queue for pending purchases
    private Queue<PurchaseEntry> purchaseQueue = new Queue<PurchaseEntry>();
    private bool isProcessingQueue = false;

    /// <summary>
    /// Public method called when player wants to buy an item.
    /// Adds it to the queue.
    /// </summary>
    public void TryBuy(ItemData item, int amount = 1, float deliveryDelay = 2f)
    {
        purchaseQueue.Enqueue(new PurchaseEntry(item, amount));

        if (!isProcessingQueue)
        {
            StartCoroutine(ProcessQueue(deliveryDelay));
        }
    }

    /// <summary>
    /// Coroutine that processes the purchase queue sequentially.
    /// </summary>
    private IEnumerator ProcessQueue(float deliveryDelay)
    {
        isProcessingQueue = true;

        while (purchaseQueue.Count > 0)
        {
            var entry = purchaseQueue.Peek();
            int totalCost = entry.item.ObolValue * entry.amount;

            // Check funds
            if (!CurrencyManager.Instance.HasEnough(totalCost))
            {
                Debug.LogWarning($"Cannot buy {entry.item.itemName}: insufficient funds.");
                purchaseQueue.Dequeue(); // Remove failed purchase
                continue;
            }

            // Spend immediately
            CurrencyManager.Instance.Spend(totalCost);

            // Notify UI or any listener
            OnItemPurchased?.Invoke(entry.item, entry.amount);

            Debug.Log($"Processing purchase of {entry.amount}x {entry.item.itemName} for {totalCost} obols...");

            // Wait for delivery delay
            yield return new WaitForSeconds(deliveryDelay);

            // Notify InventoryManager / UI that item is delivered
            OnItemDelivered?.Invoke(entry.item, entry.amount);
            Debug.Log($"Delivered {entry.amount}x {entry.item.itemName}");

            purchaseQueue.Dequeue(); // Remove processed purchase
        }

        isProcessingQueue = false;
    }

    /// <summary>
    /// Returns the number of pending purchases in the queue for a specific item.
    /// </summary>
    public int GetQueueCount(ItemData item)
    {
        int count = 0;
        foreach (var entry in purchaseQueue)
        {
            if (entry.item == item)
                count += entry.amount;
        }
        return count;
    }

    // Selling method (unchanged)
    public bool TrySell(ItemData item, GhostBehavior ghost, int amount = 1)
    {
        if (ghost.currentRequestedItem != item)
        {
            Debug.LogWarning($"{ghost.name} does not want {item.itemName}.");
            ghost.OnFailedPurchase();
            return false;
        }

        bool removed = InventoryManager.Instance.RemoveItem(item, amount);
        if (!removed)
        {
            Debug.LogWarning($"Cannot sell {item.itemName}: not enough stock.");
            ghost.OnFailedPurchaseDueToStock();
            return false;
        }

        int totalValue = item.SellValue * amount;
        CurrencyManager.Instance.Earn(totalValue);

        Debug.Log($"Sold {amount}x {item.itemName} to ghost for {totalValue} obols.");

        ghost.OnSuccessfulPurchase();
        return true;
    }
}
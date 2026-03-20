using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages buying and selling of items with queue support and delivery delays.
/// </summary>
public class TransactionManager : MonoBehaviour
{
    public static TransactionManager Instance;

    public event Action<ItemData, int> OnItemPurchased;   // Fired immediately after spending money
    public event Action<ItemData, int> OnItemDelivered;   // Fired after delivery delay

    private void Awake()
    {
        Instance = this;
    }

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

    private Queue<PurchaseEntry> purchaseQueue = new Queue<PurchaseEntry>();
    private Coroutine queueCoroutine;

    /// <summary>
    /// Adds a purchase to the queue and starts processing if not already running.
    /// </summary>
    public void TryBuy(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0) return;

        purchaseQueue.Enqueue(new PurchaseEntry(item, amount));

        if (queueCoroutine == null)
            queueCoroutine = StartCoroutine(ProcessQueue());
    }

    /// <summary>
    /// Sequentially processes queued purchases safely.
    /// </summary>
    private IEnumerator ProcessQueue()
    {
        while (purchaseQueue.Count > 0)
        {
            var entry = purchaseQueue.Peek();
            int totalCost = entry.item.ObolValue * entry.amount;

            // Check funds
            if (!CurrencyManager.Instance.HasEnough(totalCost))
            {
                Debug.LogWarning($"Cannot buy {entry.item.itemName}: insufficient funds ({totalCost} required).");
                purchaseQueue.Dequeue(); // Remove failed purchase
                continue;
            }

            // Spend immediately
            CurrencyManager.Instance.Spend(totalCost);
            OnItemPurchased?.Invoke(entry.item, entry.amount);

            // Wait for delivery time
            float deliveryTime = Mathf.Max(entry.item.deliveryTime, 0.1f);
            yield return new WaitForSeconds(deliveryTime);

            // Deliver the item
            OnItemDelivered?.Invoke(entry.item, entry.amount);
            purchaseQueue.Dequeue();
        }

        queueCoroutine = null; // allow new queue to start
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

    /// <summary>
    /// Sell an item to a ghost.
    /// </summary>
    public bool TrySell(ItemData item, GhostBehavior ghost, int amount = 1)
    {
        if (ghost == null || item == null || amount <= 0) return false;

        if (ghost.currentRequestedItem != item)
        {
            ghost.OnFailedPurchase();
            return false;
        }

        if (!InventoryManager.Instance.RemoveItem(item, amount))
        {
            ghost.OnFailedPurchaseDueToStock();
            return false;
        }

        CurrencyManager.Instance.Earn(item.SellValue * amount);
        ghost.OnSuccessfulPurchase();
        return true;
    }
}
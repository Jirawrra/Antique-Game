using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages buying and selling of items with internal queue handling.
/// </summary>
public class TransactionManager : MonoBehaviour
{
    public static TransactionManager Instance;

    // Event only for UI updates (slider, notifications)
    public event Action<ItemData, int> OnItemDelivered;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private class PurchaseEntry
    {
        public ItemData item;
        public int amount;
        public float startTime;
        public float deliveryTime;

        public PurchaseEntry(ItemData item, int amount)
        {
            this.item = item;
            this.amount = amount;
            float upgradedTime = UpgradeManager.Instance.GetModifiedDeliveryTime(item.deliveryTime);
            this.deliveryTime = Mathf.Max(upgradedTime, 0.1f);
            this.startTime = Time.time;
        }
    }

    private Queue<PurchaseEntry> purchaseQueue = new Queue<PurchaseEntry>();
    private Coroutine queueCoroutine;

    /// <summary>
    /// Enqueues a purchase; validates currency internally.
    /// </summary>
    public void TryBuy(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0) return;

        int modifiedCost = item.ObolValue;
        int totalCost = modifiedCost * amount;

        if (!CurrencyManager.Instance.HasEnough(totalCost))
        {
            Debug.LogWarning($"Cannot buy {item.itemName}: insufficient funds ({totalCost} obols).");
            AudioManager.Instance.Play("Warning");
            return; // abort safely
        }

        // Deduct currency immediately
        CurrencyManager.Instance.Spend(totalCost);
        AudioManager.Instance.Play("Credits Open");

        // Enqueue purchase
        purchaseQueue.Enqueue(new PurchaseEntry(item, amount));

        // Start processing if not already
        if (queueCoroutine == null)
            queueCoroutine = StartCoroutine(ProcessQueue());
    }

    /// <summary>
    /// Processes queued purchases sequentially.
    /// </summary>
    private IEnumerator ProcessQueue()
    {
        while (purchaseQueue.Count > 0)
        {
            var entry = purchaseQueue.Peek();

            entry.startTime = Time.time;

            yield return new WaitForSeconds(entry.deliveryTime);

            InventoryManager.Instance.AddItem(entry.item, entry.amount);
            OnItemDelivered?.Invoke(entry.item, entry.amount);

            purchaseQueue.Dequeue();
        }

        queueCoroutine = null;
    }
    /// <summary>
    /// Returns pending purchase count for a specific item.
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
    /// Sell item to a ghost; independent of purchase queue.
    /// </summary>
    public bool TrySell(ItemData item, GhostBehavior ghost, int amount = 1)
    {
        if (item == null || ghost == null || amount <= 0) return false;

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


        // Calculate earnings with upgrades
        int upgradedValue = UpgradeManager.Instance.GetModifiedSellValue(item.SellValue);
        int totalValue = upgradedValue * amount;

        CurrencyManager.Instance.Earn(totalValue);

        ghost.OnSuccessfulPurchase();
        return true;
    }

    public float GetProgress(ItemData item)
    {
        if (purchaseQueue.Count == 0)
            return 0f;

        var entry = purchaseQueue.Peek();

        if (entry.item != item)
            return 0f;

        float elapsed = Time.time - entry.startTime;
        return Mathf.Clamp01(elapsed / entry.deliveryTime);
    }
}
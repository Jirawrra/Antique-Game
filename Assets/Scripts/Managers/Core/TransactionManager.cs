using Unity.VisualScripting;
using UnityEngine;

public class TransactionManager : MonoBehaviour
{
    public static TransactionManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool TryBuy(ItemData item, int amount = 1) // Returns true if the purchase was successful, false otherwise
    {
        int totalCost = item.ObolValue * amount; // Calculate the total cost of the purchase

        if (!CurrencyManager.Instance.HasEnough(totalCost))
        {
            Debug.LogWarning($"Cannot buy {item.itemName}: insufficient funds. Need {totalCost}, have {CurrencyManager.Instance.GetBalance()}");
            return false;
        }

        bool spent = CurrencyManager.Instance.Spend(totalCost);
        if (!spent) return false;

        InventoryManager.Instance.AddItem(item, amount);
        Debug.Log($"Purchased {amount}x {item.itemName} for {totalCost}.");
        return true;
    }

    public bool TrySell(ItemData item, GhostBehavior ghost, int amount = 1)
    {
        // Check the ghost actually wants this item
        if (ghost.currentRequestedItem != item)
        {
            Debug.LogWarning($"{ghost.name} does not want {item.itemName}.");
            ghost.OnFailedPurchase();
            return false;
        }

        // Check the player has it in stock
        bool removed = InventoryManager.Instance.RemoveItem(item, amount);
        if (!removed)
        {
            Debug.LogWarning($"Cannot sell {item.itemName}: not enough stock.");
            ghost.OnFailedPurchaseDueToStock();
            return false;
        }

        // Pay the player using SELL value
        int totalValue = item.SellValue * amount;
        CurrencyManager.Instance.Earn(totalValue);

        Debug.Log($"Sold {amount}x {item.itemName} to ghost for {totalValue} obols.");

        ghost.OnSuccessfulPurchase();
        return true;
    }
}
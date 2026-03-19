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
        int totalCost = item.ObolValue * amount;

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

    public bool TrySell(ItemData item, int amount = 1) // Returns true if the sale was successful, false otherwise
    {
        bool removed = InventoryManager.Instance.RemoveItem(item, amount);
        if (!removed)
        {
            Debug.LogWarning($"Cannot sell {item.itemName}: not enough stock.");
            return false;
        }

        int totalValue = item.ObolValue * amount;
        CurrencyManager.Instance.Earn(totalValue);
        Debug.Log($"Sold {amount}x {item.itemName} for {totalValue}.");
        return true;
    }
}
using Unity.VisualScripting;
using UnityEngine;

public class TransactionManager : MonoBehaviour
{
    public InventoryManager inventory;
    public CurrencyManager currency;
    public GhostBehavior ghostManager;

    public void TryBuyItem(ItemData item)
    {
        if (currency.SpendObols(item.ObolValue))
        {
            inventory.AddItem(item);
        }
    }

    public void TrySellItem(ItemData item)
    {
        if (inventory.HasItem(item))
        {
            inventory.RemoveItem(item);
            currency.AddObols(item.ObolValue);
        }
    }
}
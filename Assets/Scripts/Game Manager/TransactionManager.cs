using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public InventoryManager inventory;
    public CurrencyManager currency;
    public GhostManager ghostManager;

    public void TrySellItem(ItemData selectedItem)
    {
        if (ghostManager.currentRequestedItem == null)
        {
            Debug.Log("No ghost present.");
            return;
        }

        if (selectedItem == ghostManager.currentRequestedItem && inventory.HasItem(selectedItem))
        {
            currency.AddObols(selectedItem.ObolValue);
            inventory.RemoveItem(selectedItem);


            Debug.Log("Item Sold!");
            ghostManager.ClearGhost();
            ghostManager.SpawnGhost();
        }
        else
        {
            Debug.Log("Wrong item.");
        }
    }
}
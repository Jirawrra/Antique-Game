using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AntiqueItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text eraText;
    [SerializeField] private TMP_Text stockText;
    [SerializeField] private Button buyButton;

    private ItemData item;

    private void Update()
    {
        if (item != null)
        {
            int stock = InventoryManager.Instance.GetStock(item); // Get the current stock of this item from the InventoryManager
            stockText.text = stock.ToString();
        }
    }

    public void Setup(ItemData itemData)
    {
        item = itemData;

        icon.sprite = item.icon;
        nameText.text = item.itemName;
        eraText.text = item.era.ToString();



        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
    }

    private void BuyItem()
    {
        //InventoryManager.Instance.AddItem(item, 1); // For testing purposes, it adds 1 item to the inventory when the buy button is clicked. This can be expanded to include price checks and other logic as needed.

        bool success = TransactionManager.Instance.TryBuy(item, 1);

        if (!success)
        {
            // Trigger a UI feedback here, e.g. shake animation or "Not enough gold" popup
            Debug.LogWarning("Transaction failed.");
        }
    }





}

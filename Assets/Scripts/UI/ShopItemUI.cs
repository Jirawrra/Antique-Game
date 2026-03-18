using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;

    ItemData itemData;

    public void Setup(ItemData item)
    {
        itemData = item;

        icon.sprite = item.icon;
        nameText.text = item.itemName;
        priceText.text = item.ObolValue.ToString();
    }
}
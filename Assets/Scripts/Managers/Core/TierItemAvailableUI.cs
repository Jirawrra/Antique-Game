using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TierItemAvailableUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;

    public void Setup(ItemData item)
    {
        icon.sprite = item.icon;
        nameText.text = item.itemName;
    }
}

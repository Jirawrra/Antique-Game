using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Shop.Upgrades
{
    public class UpgradeItem : MonoBehaviour
    {
        [Header("Upgrade Item Panel References")]
        [SerializeField] private Image upgradeItemImage;
        [SerializeField] private TMPro.TextMeshProUGUI upgradeItemTitle;
        [SerializeField] private TMPro.TextMeshProUGUI upgradeItemSubtitle;
        [SerializeField] private Button upgradeItemBuyButton;
        [SerializeField] private TMPro.TextMeshProUGUI upgradeItemBuyButtonText;

        public void Initialize(Sprite upgradeItemSprite, string upgradeItemName, string upgradeItemDescription, int upgradeItemPrice)
        {
            // Initializes assets connected to [SerializeField] into the game
            upgradeItemImage.sprite = upgradeItemSprite;
            upgradeItemTitle.text = upgradeItemName;
            upgradeItemSubtitle.text = upgradeItemDescription;
            upgradeItemBuyButtonText.text = upgradeItemPrice.ToString();
        }
    }
}
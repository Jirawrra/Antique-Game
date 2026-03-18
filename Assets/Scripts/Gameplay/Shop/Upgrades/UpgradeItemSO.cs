using UnityEngine;

namespace Gameplay.Shop.Upgrades
{
    [CreateAssetMenu(fileName = "UpgradeItemSO", menuName = "Scriptable Objects/UpgradeItemSO")]
    public class UpgradeItemSo : ScriptableObject
    {
        // Creates a scriptableObject with these parameters
        public Sprite upgradeItemSprite;
        public string upgradeItemName;
        public string upgradeItemDescription;
        public double upgradeItemPrice;
    }
}
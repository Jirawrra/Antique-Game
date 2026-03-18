using UnityEngine;

[System.Serializable]
public class ShopItemState
{
    public ItemData item;
    public float unlockTimer;
    public bool isUnlocked;

    float GetUnlockTime(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return 60f;
            case Rarity.Uncommon: return 120f;
            case Rarity.Rare: return 300f;
            case Rarity.Legendary: return 600f;
            default: return 60f;

        }
    }
}



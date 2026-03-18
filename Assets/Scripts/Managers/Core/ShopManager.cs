using System.Collections.Generic;
using Gameplay.Shop;
using UnityEditor.UIElements;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public TierData tierData;
    public List<ShopItemState> shopItems = new();

    void Start()
    {
        LoadTierItems();
    }

    void LoadTierItems()
    {
        shopItems.Clear();

        foreach (var item in tierData.allowedItems)
        {
            ShopItemState state = new ShopItemState
            {
                item = item,
                unlockTimer = GetUnlockTime(item.rarity),
                isUnlocked = false
            };

            shopItems.Add(state);
            Debug.Log("Loaded item: " + item.name);
            Debug.Log("Total shop items: " + shopItems.Count);
        }
    }

    float GetUnlockTime(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return 60f;
            case Rarity.Uncommon: return 60f;
            case Rarity.Rare: return 60f;
            case Rarity.Legendary: return 60f;
            default: return 60f;
        }
    }

    private void Update()
    {
        foreach (var item in shopItems)
        {
            if (item.isUnlocked) continue;

            item.unlockTimer -= Time.deltaTime;

            if (item.unlockTimer <= 0f)
                item.isUnlocked = true;
        }
    }
}
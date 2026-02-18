using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AntiqueSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float spawnInterval = 300f; // 5 minutes in seconds (Set to something shorter for testing eg: 60f)
    public int maxSpawnCount = 3; // Maximum number of antiques to spawn

    [Header("Item Database")]
    public List<ItemData> allGameItems; // Put all in-game items here
    private List<ItemData> validItems = new List<ItemData>(); // Items that can currently spawn via progress

    [Header("Progression")]
    public Era currentUnlockedTier = Era.Ancient; // Base Value

    private void Start()
    {
        UpdateUnlockList();
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnBatch();
        }
    }

    // Spawns multiple items per timer cycle
    void SpawnBatch()
    {
        if (validItems.Count == 0) return;

        int count = Random.Range(1, maxSpawnCount + 1);

        for (int i = 0; i < count; i++)
        {
            ItemData spawnedItem = GetRandomWeighedItem();

            if(spawnedItem != null)
            {
                Debug.Log($"Spawned: {spawnedItem.itemName} | (Rarity: {spawnedItem.rarity})");
            }
        }
    }

    ItemData GetRandomWeighedItem()
    {
        float totalWeight = 0f;

        foreach (var item in validItems)
        {
            totalWeight += GetRarityMultiplier(item.rarity);
        }

        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0f;

        foreach (var item in validItems)
        {
            currentWeight += GetRarityMultiplier(item.rarity);

            if (randomValue <= currentWeight)
            {
                return item;
            }
        }

        return null;
    }

    float GetRarityMultiplier(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return 70f; // Common
            case Rarity.Uncommon: return 20f; // Uncommon
            case Rarity.Rare: return 8f; // Rare
            case Rarity.Legendary: return 1f; // Legendary
            default: return 1f;
        }
    }

    public void UpdateUnlockList()
    {
        validItems.Clear();

        foreach (var item in allGameItems)
        {
            if (item.era <= currentUnlockedTier)
            {
                validItems.Add(item);
            }
        }
    }
}
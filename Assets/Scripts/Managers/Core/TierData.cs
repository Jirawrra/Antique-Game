using UnityEngine;

[CreateAssetMenu(menuName = "Idle/Tier Data")]
public class TierData : ScriptableObject
{
    public int tierLevel;

    [Header("Customers")]
    public int maxGhosts;
    public Vector2 spawnIntervalRange;

    [Header("Items Available In This Tier")]
    public ItemData[] allowedItems;

    [Header("Ghost Available In This Tier")]
    public GhostData[] allowedGhosts;

    [Header("CoinsAquiredNeededToUnlockNextTier")]
    public int coinsNeededToUnlockNextTier;



}
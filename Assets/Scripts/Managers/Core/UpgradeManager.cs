using UnityEngine;
using System;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Upgrade Costs")]
    [SerializeField] private int baseSellCost = 100;
    [SerializeField] private int baseSpeedCost = 150;

    [SerializeField] private float costMultiplier = 1.5f;

    public int GetSellLevel() => sellLevel;
    public int GetSpeedLevel() => speedLevel;

    private int sellLevel = 0;
    private int speedLevel = 0;

    public event Action OnUpgradeChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // =========================
    // UPGRADE METHODS
    // =========================
    public bool UpgradeSellValue()
    {
        int cost = GetSellUpgradeCost();

        if (!CurrencyManager.Instance.HasEnough(cost))
        {
            Debug.Log("Not enough obols for Sell Upgrade");
            AudioManager.Instance.Play("Warning");
            return false;
        }

        CurrencyManager.Instance.Spend(cost);

        sellLevel++;
        OnUpgradeChanged?.Invoke();

        return true;
    }

    public bool UpgradeDeliverySpeed()
    {
        int cost = GetSpeedUpgradeCost();

        if (!CurrencyManager.Instance.HasEnough(cost))
        {
            Debug.Log("Not enough obols for Speed Upgrade");
            AudioManager.Instance.Play("Warning");
            return false;
        }

        CurrencyManager.Instance.Spend(cost);

        speedLevel++;
        OnUpgradeChanged?.Invoke();

        return true;
    }

    // =========================
    // MODIFIERS
    // =========================

    public int GetModifiedSellValue(int baseValue)
    {
        // +25% per level (tweakable)
        return Mathf.RoundToInt(baseValue * (1f + sellLevel * 0.25f));
    }

    public float GetModifiedDeliveryTime(float baseTime)
    {
        // -15% per level
        float multiplier = 1f - (speedLevel * 0.15f);
        multiplier = Mathf.Clamp(multiplier, 0.3f, 1f);
        return baseTime * multiplier;
    }

    public int GetSellUpgradeCost()
    {
        return Mathf.RoundToInt(baseSellCost * Mathf.Pow(costMultiplier, sellLevel));
    }

    public int GetSpeedUpgradeCost()
    {
        return Mathf.RoundToInt(baseSpeedCost * Mathf.Pow(costMultiplier, speedLevel));
    }


}
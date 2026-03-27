using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    [Header("Sell Upgrade UI")]
    [SerializeField] private TMP_Text sellLevelText;
    [SerializeField] private TMP_Text sellCostText;
    [SerializeField] private Button sellButton;

    [Header("Speed Upgrade UI")]
    [SerializeField] private TMP_Text speedLevelText;
    [SerializeField] private TMP_Text speedCostText;
    [SerializeField] private Button speedButton;

    private void Awake()
    {
        // Button listeners
        sellButton.onClick.AddListener(OnSellUpgrade);
        speedButton.onClick.AddListener(OnSpeedUpgrade);
    }

    private void OnEnable()
    {
        if (UpgradeManager.Instance != null)
            UpgradeManager.Instance.OnUpgradeChanged += RefreshUI;

        RefreshUI(); // Always refresh when opened
    }

    private void OnDisable()
    {
        if (UpgradeManager.Instance != null)
            UpgradeManager.Instance.OnUpgradeChanged -= RefreshUI;
    }

    // =========================
    // BUTTON ACTIONS
    // =========================

    private void OnSellUpgrade()
    {
        UpgradeManager.Instance.UpgradeSellValue();
    }

    private void OnSpeedUpgrade()
    {
        UpgradeManager.Instance.UpgradeDeliverySpeed();
    }

    // =========================
    // UI REFRESH
    // =========================

    private void RefreshUI()
    {
        if (UpgradeManager.Instance == null) return;

        // LEVELS
        sellLevelText.text = "Lv. " + UpgradeManager.Instance.GetSellLevel();
        speedLevelText.text = "Lv. " + UpgradeManager.Instance.GetSpeedLevel();

        // COSTS
        int sellCost = UpgradeManager.Instance.GetSellUpgradeCost();
        int speedCost = UpgradeManager.Instance.GetSpeedUpgradeCost();

        sellCostText.text = sellCost.ToString();
        speedCostText.text = speedCost.ToString();

        // BUTTON INTERACTABLE (disable if not enough money)
        sellButton.interactable = CurrencyManager.Instance.HasEnough(sellCost);
        speedButton.interactable = CurrencyManager.Instance.HasEnough(speedCost);
    }

    // =========================
    // HELPERS (TEMP if private)
    // =========================

    private int GetSellLevel()
    {
        // If you make levels public later, replace this
        return typeof(UpgradeManager)
            .GetField("sellLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(UpgradeManager.Instance) as int? ?? 0;
    }

    private int GetSpeedLevel()
    {
        return typeof(UpgradeManager)
            .GetField("speedLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(UpgradeManager.Instance) as int? ?? 0;
    }
}
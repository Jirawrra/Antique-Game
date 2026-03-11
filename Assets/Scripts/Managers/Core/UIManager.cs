using UnityEngine;
using System;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    private enum Tab { None, Antiques, Upgrades }
    public static event Action OnNotificationClosed;
    public static event Action OnNotificationOpened;

    [Header("Notification")]
    [SerializeField] private GameObject notificationPanel;

    [Header("Panels")]
    [SerializeField] private GameObject antiquesPanel;
    [SerializeField] private GameObject upgradesPanel;
    [SerializeField] private PanelSlideMove panelMover;

    [Header("Currency UI")]
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private TextMeshProUGUI obolsText;
    [SerializeField] private TextMeshProUGUI drachmaText;

    [Header("Shop UI")]
    [SerializeField] private Transform antiquesContentParent;
    [SerializeField] private GameObject shopItemPrefab;

    [SerializeField] private ShopManager shopManager;

    private Tab currentTab = Tab.None;

    void Start()
    {
        if (currencyManager != null)
        {
            currencyManager.OnObolsChanged += UpdateObolsUI;
            currencyManager.OnDrachmaChanged += UpdateDrachmaUI;
        }
    }

    void OnDestroy()
    {
        if (currencyManager != null)
        {
            currencyManager.OnObolsChanged -= UpdateObolsUI;
            currencyManager.OnDrachmaChanged -= UpdateDrachmaUI;
        }
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Space key pressed - toggling notification");
            notificationPanel.SetActive(true);
            OnNotificationOpened?.Invoke();
        }
    }

    void BuildAntiquesUI()
    {
        if (shopManager == null)
        {
            Debug.LogError("ShopManager reference missing in UIManager");
            return;
        }

        if (shopItemPrefab == null)
        {
            Debug.LogError("ShopItemPrefab not assigned in UIManager");
            return;
        }

        if (antiquesContentParent == null)
        {
            Debug.LogError("AntiquesContentParent not assigned in UIManager");
            return;
        }

        // Clear existing UI
        foreach (Transform child in antiquesContentParent)
            Destroy(child.gameObject);

        // Build UI for each shop item
        foreach (var itemState in shopManager.shopItems)
        {
            if (itemState.item == null) continue;

            Debug.Log("Spawning shop item: " + itemState.item.itemName);

            GameObject slot = Instantiate(shopItemPrefab, antiquesContentParent);

            ShopItemUI ui = slot.GetComponent<ShopItemUI>();
            if (ui != null)
                ui.Setup(itemState.item);
            else
                Debug.LogError("ShopItemUI script missing on prefab!");
        }

        Debug.Log("Shop items count: " + shopManager.shopItems.Count);
    }

    public void CloseNotification()
    {
        OnNotificationClosed?.Invoke();
    }

    public void OnAntiques(bool isOn)
    {
        if (!isOn) return;
        HandleTab(Tab.Antiques);
    }

    public void OnUpgrades(bool isOn)
    {
        if (!isOn) return;
        HandleTab(Tab.Upgrades);
    }

    private void HandleTab(Tab clickedTab)
    {
        if (currentTab == clickedTab) // Toggle the same tab
        {
            if (panelMover.IsUp)
                panelMover.SlideDown();
            else
                panelMover.SlideUp();

            return;
        }

        currentTab = clickedTab;

        antiquesPanel.SetActive(clickedTab == Tab.Antiques);
        if (clickedTab == Tab.Antiques)
            BuildAntiquesUI();

        upgradesPanel.SetActive(clickedTab == Tab.Upgrades);

        if (!panelMover.IsUp)
            panelMover.SlideUp();
    }

    private void UpdateObolsUI(int amount)
    {
        if (obolsText != null)
            obolsText.text = amount.ToString();
    }

    private void UpdateDrachmaUI(int amount)
    {
        if (drachmaText != null)
            drachmaText.text = amount.ToString();
    }
}
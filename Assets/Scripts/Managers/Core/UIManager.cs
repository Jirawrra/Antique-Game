using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

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
        currencyManager.OnObolsChanged += UpdateObolsUI;
        currencyManager.OnDrachmaChanged += UpdateDrachmaUI;
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
        foreach (Transform child in antiquesContentParent) //Clear existing UI
            Destroy(child.gameObject);

        foreach (var itemState in shopManager.shopItems) // Create UI for each item that gets added
        {
            Debug.Log("Spawning shop item: " + itemState.item.itemName);
            GameObject slot = Instantiate(shopItemPrefab, antiquesContentParent);

            ShopItemUI ui = slot.GetComponent<ShopItemUI>();
            
            if (ui != null)
            {
                ui.Setup(itemState.item);
            }
        }

        Debug.Log("Shop items count: " + shopManager.shopItems.Count);
    }

    public void closeNotification()
    {
        // notificationPanel.SetActive(false);
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

        antiquesPanel.SetActive(clickedTab == Tab.Antiques); // Show Antiques panel if selected
        if (clickedTab == Tab.Antiques)
        {
            BuildAntiquesUI();
        }

        upgradesPanel.SetActive(clickedTab == Tab.Upgrades); // Show Upgrades panel if selected

        //Slide Up if the panel is not already up
        if (!panelMover.IsUp)
            panelMover.SlideUp();
    }

    private void UpdateObolsUI(int amount)
    {
        obolsText.text = amount.ToString();
    }

    private void UpdateDrachmaUI(int amount)
    {
        drachmaText.text = amount.ToString();
    }

}

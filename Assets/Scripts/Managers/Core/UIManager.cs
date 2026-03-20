using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers.Core
{
    public class UIManager : MonoBehaviour
    {
        private enum Tab { None, Antiques, Upgrades }
        public static event Action OnNotificationClosed;
        public static event Action OnNotificationOpened;

        private Tab currentTab = Tab.None;

        [SerializeField] private ShopManager shopManager;

        [Header("Currency UI")]
        //  [SerializeField] private CurrencyManager currencyManager;
        [SerializeField] private TextMeshProUGUI obolsText;
        [SerializeField] private TextMeshProUGUI drachmaText;

        [Header("Notification")]
        [SerializeField] private GameObject notificationPanel;

        [Header("Shop Panels")]
        [SerializeField] private GameObject antiquesPanel;
        [SerializeField] private GameObject upgradesPanel;
        [SerializeField] private PanelSlideMove panelMover;

        [Header("PremiumShop Panel")]
        [SerializeField] private GameObject premiumShopPanel;

        [Header("Black background Panel")]
        [SerializeField] private GameObject backgroundOverlay;



        void Start()
        {
            CurrencyManager.Instance.OnObolsChanged += UpdateObolsUI;
            //CurrencyManager.Instance.OnDrachmaChanged += UpdateDrachmaUI;
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

        public void CloseNotification()
        {
            // notificationPanel.SetActive(false);
            OnNotificationClosed?.Invoke();
        }

        public void OnPremiumShop()
        {
            Debug.Log("Premium Shop Opened");
            premiumShopPanel.SetActive(true);
            backgroundOverlay.SetActive(true);
        }

        public void OnPremiumShopClose()
        {
            backgroundOverlay.SetActive(false);
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
}
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private enum Tab { None, Antiques, Upgrades }
    public static event Action OnNotificationClosed;
    public static event Action OnNotificationOpened;

    [SerializeField] private GameObject notificationPanel;

    [SerializeField] private GameObject antiquesPanel;
    [SerializeField] private GameObject upgradesPanel;
    [SerializeField] private PanelSlideMove panelMover;

    private Tab currentTab = Tab.None;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            notificationPanel.SetActive(true);
            OnNotificationOpened?.Invoke();
        }
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
        upgradesPanel.SetActive(clickedTab == Tab.Upgrades); // Show Upgrades panel if selected

        //Slide Up if the panel is not already up
        if (!panelMover.IsUp)
            panelMover.SlideUp();
    }

}

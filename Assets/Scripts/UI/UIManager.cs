using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;
using System.Net.Mail;



public class UIManager : MonoBehaviour
{
    public static event Action OnNotificationClosed;
    public static event Action OnNotificationOpened;

    public GameObject notificationPanel;

    void Start()
    {

    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            notificationPanel.SetActive(true);
            OnNotificationOpened?.Invoke();
            Debug.Log("Notification is Called");
        }
    }

    public void Upgrades()
    {
        Debug.Log("Show Upgrades UI");

    }

    public void Antique()
    {
        Debug.Log("Show Antique UI");
    }
    public void Yes()
    {
        Debug.Log("Yes");

        //  NotificationPanel.SetActive(false);
        OnNotificationClosed?.Invoke();
    }

}

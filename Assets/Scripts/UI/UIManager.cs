using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    //public Image UpgradeImage;
     public float duration = 0.25f;
    private RectTransform rect;

    public GameObject NotificationPanel;

      public static event Action OnNotificationClosed;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        
    }

    public void Upgrades()
    {
        Debug.Log ("Show Upgrades UI");
            
    }

    public void Antique()
    {
        Debug.Log ("Show Antique UI");
    }
    public void Yes()
    {
        Debug.Log ("Yes");

      //  NotificationPanel.SetActive(false);
        OnNotificationClosed?.Invoke();
    }

}

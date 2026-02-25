using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class MainMenuUIManager : MonoBehaviour
{

    [SerializeField] private GameObject creditsPanel;

    
    public static event Action OnCreditsPanelClosed;
    public static event Action OnCreditsPanelOpened;

    public void StartGame()
    {
        //loads game scene
        SceneLoader.Instance.LoadScene("Main Level");
    }

    public void CreditsPanelOpened()
    {
            creditsPanel.SetActive(true);
            OnCreditsPanelOpened?.Invoke(); 
            Debug.Log("Credits Panel Opened");
    }

    public void CreditsPanelClosed()
    {
        //creditsPanel.SetActive(false);
        OnCreditsPanelClosed?.Invoke();
        Debug.Log("Credits Panel Closed");
        

    }

}

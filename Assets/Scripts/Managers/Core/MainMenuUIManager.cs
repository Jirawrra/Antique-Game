using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class MainMenuUIManager : MonoBehaviour
{

    [SerializeField] private GameObject creditsPanel;

    [Tooltip("The dark background panel (Background Shift)")]
    [SerializeField] private GameObject backgroundOverlay;
    public static event Action OnCreditsPanelClosed;
    public static event Action OnCreditsPanelOpened;

    public void StartGame()
    {
        //loads game scene
        SceneLoader.Instance.LoadScene("Main Level");
        AudioManager.Instance.Play("Play Game");



    }


    public void CreditsPanelOpened()
    {
        if (backgroundOverlay != null) backgroundOverlay.SetActive(true);
        creditsPanel.SetActive(true);
        OnCreditsPanelOpened?.Invoke();
        Debug.Log("Credits Panel Opened");

        AudioManager.Instance.Play("Credits Open");
    }

    public void CreditsPanelClosed()
    {

        if (backgroundOverlay != null) backgroundOverlay.SetActive(false);
        //creditsPanel.SetActive(false);
        OnCreditsPanelClosed?.Invoke();
        Debug.Log("Credits Panel Closed");
        AudioManager.Instance.Play("Exit Button");


    }

}

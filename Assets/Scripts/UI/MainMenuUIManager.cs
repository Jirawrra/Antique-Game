using UnityEngine;


public class MainMenuUIManager : MonoBehaviour
{

    [SerializeField] private CreditsPanel creditsPanel;

    public void OpenCredits()
    {
        creditsPanel.OpenCredits();
    }

    public void CloseCredits()
    {
        creditsPanel.CloseCredits();
    }

    public void StartGame()
    {
        //loads game scene
        SceneLoader.Instance.LoadScene("Main Level");
    }


}

using UnityEngine;

public class CreditsPanel : MonoBehaviour
{
    [SerializeField] GameObject creditsPanel;

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
}

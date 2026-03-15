using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("References")]
    public GameObject tutorialButton;       // The button shown at game start
    public GameObject tutorialPanel;        // The UI panel shown during tutorial
    public TutorialAnimationPopIn spritePopIn;         // Your sprite pop-in script

    [Header("Settings")]
    public float delayBeforeSprite = 0.5f;  // Pause before sprite slides in

    private bool isTutorialRunning = false;

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // Show the trigger button, hide tutorial panel
        tutorialButton.SetActive(true);
        tutorialPanel.SetActive(false);
    }

    // --- Called by the tutorial button's OnClick ---
    public void OnTutorialButtonPressed()
    {
        if (isTutorialRunning) return;
        StartCoroutine(RunTutorial());
    }

    IEnumerator RunTutorial()
    {
        isTutorialRunning = true;

        // 1. Hide the trigger button
        tutorialButton.SetActive(false);

        // 4. Show tutorial UI panel
        tutorialPanel.SetActive(true);
        Debug.Log("TutorialPanel Opened");

        // 5. Wait briefly, then trigger sprite pop-in
        yield return new WaitForSecondsRealtime(delayBeforeSprite);
        spritePopIn.PlayAnimation();

    }

    public void EndTutorial()
    {
        if (!isTutorialRunning) return;
        StartCoroutine(FinishTutorial());
    }

    IEnumerator FinishTutorial()
    {
        tutorialPanel.SetActive(false);

        isTutorialRunning = false;

        yield return null;
    }


}
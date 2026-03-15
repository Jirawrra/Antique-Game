using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("References")]
    public GameObject tutorialButton;
    public GameObject tutorialPanel;
    public TutorialAnimationPopIn spritePopIn;

    [Header("Settings")]
    public float delayBeforeSprite = 0.5f;

    [Header("Tutorial Content")]
    public TutorialData tutorialData;           // Drag your TutorialData SO here
    public TextMeshProUGUI stepText;            // TMP text inside panel
    public Image stepImage;                     // Image inside panel
    public GameObject skipButton;               // Skip button
    public GameObject finishButton;


    private int currentStep = 0;
    private bool isTutorialRunning = false;

    void Awake()
    {

        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {

        tutorialButton.SetActive(true);
        tutorialPanel.SetActive(false);
    }


    public void OnTutorialButtonPressed()
    {
        if (isTutorialRunning) return;
        StartCoroutine(RunTutorial());
    }

    IEnumerator RunTutorial()
    {
        isTutorialRunning = true;
        currentStep = 0;
        tutorialButton.SetActive(false);


        yield return new WaitForSecondsRealtime(delayBeforeSprite);
        tutorialPanel.SetActive(true);
        Debug.Log("TutorialPanel Opened");
        spritePopIn.PlayAnimation();

    }

    public void OnSkipPressed()
    {
        currentStep++;

        if (currentStep >= tutorialData.steps.Length)
        {
            EndTutorial();
            return;
        }

        ShowStep(currentStep);
    }

    void ShowStep(int index)
    {
        TutorialStepData step = tutorialData.steps[index];

        // Update content
        stepText.text = step.stepText;
        stepImage.sprite = step.stepImage;

        // Show Finish button on last step, Skip otherwise
        bool isLastStep = index >= tutorialData.steps.Length - 1;
        skipButton.SetActive(!isLastStep);
        finishButton.SetActive(isLastStep);
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
        spritePopIn.PlayOutAnimation();

        yield return null;
    }


}
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStep", menuName = "Tutorial/Step")]
public class TutorialStepData : ScriptableObject
{
    public string stepText;
    public Sprite stepImage;
}
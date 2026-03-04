using UnityEngine;

[CreateAssetMenu(fileName = "GhostData", menuName = "Scriptable Objects/GhostData")]
public class GhostData : ScriptableObject
{

    [Header("Identity")]
    public int ghostID;
    public string ghostName;

    [Header("Visual")]
    public Sprite ghostSprite;

    [Header("Behavior")]
    public int coinReward;
    public int patienceLevel;



}

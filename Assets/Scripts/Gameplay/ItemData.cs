using UnityEngine;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Legendary
}

public enum Era
{
    Ancient,
    PostClassical,
    EarlyModern,
    Modern
}

[CreateAssetMenu(fileName = "NewAntique", menuName = "Antique Shop/Item")]
public class  ItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemName;
    [TextArea]
    public string description;
    public Sprite icon;

    [Header("Game Data")]
    public Era era;        // Era of the item
    public Rarity rarity;  // Spawn Rarity
    public int ObolValue;  // Item Value
}
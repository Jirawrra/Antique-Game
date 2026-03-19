using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class AntiqueItemsManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private AntiqueItemUI itemEntryPrefab;

    [Header("Data References")]
    [SerializeField] private GameMaster gameMaster;

    [SerializeField] private List<GameObject> spawnedEntries = new List<GameObject>();
    private void OnEnable()
    {
        if (GameMaster.Instance != null)
            GameMaster.Instance.OnTierChanged += Refresh;
    }

    private void OnDisable()
    {
        if (GameMaster.Instance != null)
            GameMaster.Instance.OnTierChanged -= Refresh;
    }

    private void Start()
    {
        if (GameMaster.Instance != null)
        {
            GameMaster.Instance.OnTierChanged -= Refresh;
            GameMaster.Instance.OnTierChanged += Refresh;
        }
    }

    private void Refresh(TierData tier)
    {

        ClearList(); //clear the existing list
        BuildList(tier.allowedItems); // replace the previous list with new allowed items according to tier
    }


    private void BuildList(ItemData[] items) // placing the available item list to the prefab container 
    {
        foreach (ItemData item in items)
        {
            AntiqueItemUI entry = Instantiate(itemEntryPrefab, contentParent);
            entry.gameObject.SetActive(true);
            entry.Setup(item);

            entry.gameObject.name = "Item_" + item.itemName; //rename the game object according to its itemData Component
            spawnedEntries.Add(entry.gameObject);
        }
    }

    private void ClearList()
    {
        foreach (GameObject obj in spawnedEntries)

            Destroy(obj);

        spawnedEntries.Clear();
    }
}
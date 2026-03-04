using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class LevelUpNotification : MonoBehaviour
{
    [Header("UI References")]
    // [SerializeField] private GameObject panel;
    [SerializeField] private Text tierNameText;

    [SerializeField] private Transform contentParent; // ScrollView/Content
    [SerializeField] private TierItemAvailableUI itemEntryPrefab;


    [SerializeField] private List<GameObject> spawnedEntries = new();


    [Header("References")]
    [SerializeField] private GameMaster gameMaster;


    private void OnEnable()
    {
        if (GameMaster.Instance != null)
        {
            GameMaster.Instance.OnTierChanged += Refresh;
        }
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
        gameObject.SetActive(true);
        ClearList();
        BuildList(tier.allowedItems);



    }

    private void BuildList(ItemData[] items)
    {
        foreach (ItemData item in items)
        {
            TierItemAvailableUI entry =
                Instantiate(itemEntryPrefab, contentParent);

            entry.Setup(item);
            spawnedEntries.Add(entry.gameObject);
        }
    }

    private void ClearList()
    {
        foreach (GameObject obj in spawnedEntries)
            Destroy(obj);

        spawnedEntries.Clear();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }



}

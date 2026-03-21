using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Managers.Core;
using TMPro;
public class LevelUpNotification : MonoBehaviour
{
    [Header("UI References")]

    [SerializeField] private GameObject backgroundOverlay;
    [SerializeField] private GameObject levelUpNotificationPanel;
    [SerializeField] private TMP_Text tierNameText;
    [SerializeField] private Transform contentParent;
    [SerializeField] private TierItemAvailableUI itemEntryPrefab;

    [Header("Behavior References")]
    public UIAnimator levelUpAnimator;

    [SerializeField] private List<GameObject> spawnedEntries = new();


    [Header("Data References")]
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
        levelUpNotificationPanel.SetActive(true);
        backgroundOverlay.SetActive(true);
        tierNameText.text = tier.TierName;
        ClearList(); //clear the existing list
        BuildList(tier.newAllowedItems); // replace the previous list with new allowed items according to tier
    }


    private void BuildList(ItemData[] items) // placing the available item list to the prefab container 
    {
        foreach (ItemData item in items)
        {
            TierItemAvailableUI entry = Instantiate(itemEntryPrefab, contentParent);

            entry.gameObject.SetActive(true);
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
        backgroundOverlay.SetActive(false);
        levelUpAnimator.AnimateClose();
    }



}

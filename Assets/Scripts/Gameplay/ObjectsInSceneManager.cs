using UnityEngine;
using System;

[Serializable]
public class TierObjectReference
{
    public TierData tierData;
    public GameObject[] objectsToShow;
}

public class ObjectsInSceneManager : MonoBehaviour
{
    [Header("Tier Object References")]
    [SerializeField] private TierObjectReference[] tierReferences;

    private void OnEnable()
    {
        if (GameMaster.Instance != null)
            GameMaster.Instance.OnTierChanged += HandleTierChanged;
    }

    private void OnDisable()
    {
        if (GameMaster.Instance != null)
            GameMaster.Instance.OnTierChanged -= HandleTierChanged;
    }

    private void Start()
    {
        if (GameMaster.Instance != null)
            HandleTierChanged(GameMaster.Instance.CurrentTier);
    }

    private void HandleTierChanged(TierData tierData)
    {
        foreach (var reference in tierReferences)
        {
            if (reference.tierData == null) continue;

            bool isCurrentTier = reference.tierData == tierData;

            // Only activate objects for the current tier, never deactivate
            if (isCurrentTier)
            {
                foreach (var obj in reference.objectsToShow)
                {
                    if (obj == null) continue;
                    obj.SetActive(true);
                }
            }
        }
    }
}
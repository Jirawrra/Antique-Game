using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

public class GhostBehavior : MonoBehaviour
{
    [Header("References")]
    private GhostSpawner spawner;

    [Header("Data")]
    private GhostData ghostData;
    public List<ItemData> allItems;

    //[HideInInspector]
    public ItemData currentRequestedItem;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Image AntiqueImage;

    public void Init(GhostSpawner ghostSpawner, GhostData data)
    {
        spawner = ghostSpawner;
        ghostData = data;

        ApplyData();
    }

    private void ApplyData()
    {
        if (ghostData == null) return;

        spriteRenderer.sprite = ghostData.ghostSprite;
    }

    void Awake()
    {
        if (allItems != null && allItems.Count > 0)
        {
            RequestRandomItem();
        }
        else
        {
            Debug.LogWarning("GhostManager: allItems list is null or empty! Assign it in the Inspector.");
        }
    }

    public void RequestRandomItem()
    {
        if (allItems == null || allItems.Count == 0) return;
        currentRequestedItem = allItems[Random.Range(0, allItems.Count)];  //selects random item from list of all items
        Debug.Log("Ghost wants: " + currentRequestedItem.itemName);

        AntiqueImage.sprite = currentRequestedItem.icon; // Update the UI image to show the requested item

    }

    public void ClearGhost()
    {
        currentRequestedItem = null;
    }

    public void Leave()
    {
        spawner.OnGhostRemoved();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Safety net
        if (spawner != null)
            spawner.OnGhostRemoved();
    }

}

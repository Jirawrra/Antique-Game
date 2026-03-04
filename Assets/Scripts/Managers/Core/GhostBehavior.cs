using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

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

    private void BuyItem()
    {

        // when transaction manager validates the purchase, it should call this method to let the ghost know the item was bought and they can leave      

    }

    public void ClearGhost()
    {
        currentRequestedItem = null;
    }


    public void OnSuccessfulPurchase()
    {
        // This method should be called by the transaction manager when the player successfully buys the requested item for this ghost
        // You can add any additional logic here (like playing a happy animation, giving rewards, etc.) before the ghost leaves

        //give coin reward to player as consolation for failed purchase
        Leave();
    }

    public void OnFailedPurchase()
    {
        // count patience level down by 1, if it reaches 0, the ghost leaves in disappointment
        // ghostData.patienceLevel

        // This method can be called by the transaction manager if the purchase fails (e.g., not enough coins)
        // You can add any logic here for what happens when a purchase fails (like playing a sad animation, reducing patience, etc.)

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

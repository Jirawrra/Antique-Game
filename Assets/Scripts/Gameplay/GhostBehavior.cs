using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class GhostBehavior : MonoBehaviour
{
    [Header("References")]
    private GhostSpawner spawner;
    [SerializeField] private GameMaster gameMaster;
    [SerializeField] private GhostData ghostData;

    [Header("Debug (Read Only)")]

    [SerializeField] private TierData debugCurrentTier;
    [SerializeField] private List<ItemData> debugAllowedItems = new();

    //[HideInInspector]
    public ItemData currentRequestedItem;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer AntiqueImage;

    [Header("Patience")]
    [SerializeField] private GameObject irritatedSprite;
    [SerializeField] private int currentPatience;
    private float patienceTimer = 0f;
    public float patienceDrainPerSecond = 1f;
    public void Init(GhostSpawner ghostSpawner, GameMaster gameMaster, GhostData data)
    {
        this.spawner = ghostSpawner;
        this.gameMaster = gameMaster;
        this.ghostData = data;

        //See which items are allowed in the current tier
        debugCurrentTier = gameMaster.CurrentTier;
        currentPatience = ghostData.patienceLevel;

        ApplyData();
        RequestRandomItem();
    }

    private void ApplyData()
    {
        if (ghostData == null) return;

        spriteRenderer.sprite = ghostData.ghostSprite;
    }

    public void RequestRandomItem()
    {

        TierData tier = gameMaster.CurrentTier; // Get the current tier from the GameMaster to know which items are allowed for this ghost to request
        ItemData[] allowedItems = tier.allowedItems;

        debugAllowedItems.Clear();
        debugAllowedItems.AddRange(tier.allowedItems);

        currentRequestedItem =
            allowedItems[Random.Range(0, allowedItems.Length)];

        Debug.Log("Ghost wants: " + currentRequestedItem.itemName);

        AntiqueImage.sprite = currentRequestedItem.icon;


    }

    private void Update()
    {
        TickPatience();
    }

    private void TickPatience()
    {
        if (currentPatience <= 0)
            return;

        patienceTimer += Time.deltaTime; // accumulate seconds

        if (patienceTimer >= 1f) // 1 second per patience point
        {
            patienceTimer = 0f;
            currentPatience--;
            Debug.Log("Patience: " + currentPatience);

            UpdateIrritatedState(); // Show irritated sprite if patience is low

            if (currentPatience <= 0)
            {
                OnFailedPurchase();
            }
        }
    }

    private void UpdateIrritatedState()
    {
        if (irritatedSprite == null || ghostData == null)
            return;

        float patiencePercent = (float)currentPatience / ghostData.patienceLevel;

        if (patiencePercent <= 0.25f) // below 25%
        {
            if (!irritatedSprite.activeSelf)
                irritatedSprite.SetActive(true); // show sprite
        }
        else
        {
            if (irritatedSprite.activeSelf)
                irritatedSprite.SetActive(false); // hide sprite
        }
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
        Debug.Log("Ghost has run out of patience and is leaving!");
        Leave();

    }

    public void Leave()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.OnGhostRemoved();
    }

}

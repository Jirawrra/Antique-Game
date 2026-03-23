using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

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
    [SerializeField] private GameObject sellButtonCanvas;

    [Header("Hover")]
    [SerializeField] private Color hoverColor = new Color(0.8f, 0.8f, 0.8f, 1f); // light grey tint
    private Color defaultColor;

    [Header("Patience")]
    [SerializeField] private GameObject irritatedSprite;
    [SerializeField] private int currentPatience;
    private float patienceTimer = 0f;
    public float patienceDrainPerSecond = 1f;

    [Header("Indicators")]
    [SerializeField] private GameObject noStockIndicator;






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

    private void Start()
    {
        defaultColor = spriteRenderer.color;
        sellButtonCanvas.SetActive(false);

        // EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        // EventTrigger.Entry onClick = new EventTrigger.Entry();
        // onClick.eventID = EventTriggerType.PointerClick;
        // onClick.callback.AddListener((_) => OnGhostClicked());
        // trigger.triggers.Add(onClick);
    }


    private void OnHoverEnter()
    {
        Debug.Log("Mouse Enter");
        spriteRenderer.color = hoverColor;
    }

    private void OnHoverExit()
    {
        Debug.Log("Mouse Exit");
        spriteRenderer.color = defaultColor;
    }

    private void OnGhostClicked()
    {
        Debug.Log("Ghost Clicked");
        if (GhostSelector.GetSelectedGhost() == this)
            GhostSelector.SelectGhost(null);
        else
            GhostSelector.SelectGhost(this);
    }

    public void SetSelected(bool selected)
    {
        sellButtonCanvas.SetActive(selected);
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

    public void BuyItem()
    {
        TransactionManager.Instance.TrySell(currentRequestedItem, this);
    }

    public void ClearGhost()
    {
        currentRequestedItem = null;
    }


    public void OnSuccessfulPurchase()
    {
        Debug.Log("Ghost is happy with the purchase!");

        int value = currentRequestedItem.SellValue;

        FloatingTextManager.Instance.Spawn(
            $"+{value}",
            transform.position

        );

        Leave();
    }

    public void OnFailedPurchase()
    {
        Debug.Log("Ghost has run out of patience and is leaving!");
        Leave();
    }

    public void OnFailedPurchaseDueToStock()
    {
        Debug.Log("Ghost is disappointed due to lack of stock!");

        if (noStockIndicator != null)
        {
            noStockIndicator.SetActive(true);
            StartCoroutine(HideNoStockIndicator());
        }

        currentPatience -= 3;
        currentPatience = Mathf.Max(0, currentPatience);

        UpdateIrritatedState();

        if (currentPatience <= 0)
            OnFailedPurchase();
    }

    private IEnumerator HideNoStockIndicator()
    {
        SpriteRenderer sr = noStockIndicator.GetComponent<SpriteRenderer>();

        if (sr == null)
            yield break;

        float duration = 0.5f;
        float timer = 0f;

        yield return new WaitForSeconds(1.5f);

        Color originalColor = sr.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float alpha = 1 - (timer / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        // Reset for next use
        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        noStockIndicator.SetActive(false);
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

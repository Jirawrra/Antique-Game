using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AntiqueItemUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text eraText;
    [SerializeField] private TMP_Text stockText;
    [SerializeField] private Button buyButton;

    [Header("Processing UI")]
    [SerializeField] private GameObject processingPanel;
    [SerializeField] private Slider processingSlider;       // Slider for delivery progress
    [SerializeField] private TMP_Text queueCountText;      // Shows how many pending purchases
    [SerializeField] private float deliveryDelay = 2f;

    private ItemData item;

    private void Update()
    {
        if (item != null)
        {
            int stock = InventoryManager.Instance.GetStock(item);
            stockText.text = stock.ToString();

            // Show pending purchases from queue
            int pending = TransactionManager.Instance.GetQueueCount(item);
            queueCountText.text = pending > 0 ? pending.ToString() : "";
        }
    }

    public void Setup(ItemData itemData)
    {
        item = itemData;

        icon.sprite = item.icon;
        nameText.text = item.itemName;
        eraText.text = item.era.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);

        // Initialize processing UI
        if (processingSlider != null)
            processingSlider.value = 0f;

        if (processingPanel != null)
            processingPanel.SetActive(false);
    }

    private void BuyItem()
    {
        if (item == null) return;

        int cost = item.ObolValue;
        if (!CurrencyManager.Instance.HasEnough(cost))
        {
            Debug.LogWarning($"Not enough obols to buy {item.itemName}");
            return;
        }

        // Show processing UI
        if (processingPanel != null)
            processingPanel.SetActive(true);

        // Subscribe to delivery event
        TransactionManager.Instance.OnItemDelivered -= OnItemDelivered;
        TransactionManager.Instance.OnItemDelivered += OnItemDelivered;

        // Trigger purchase
        TransactionManager.Instance.TryBuy(item, 1, deliveryDelay);

        // Start slider coroutine
        StartCoroutine(UpdateSlider(deliveryDelay));
    }

    private IEnumerator UpdateSlider(float duration)
    {
        if (processingSlider == null) yield break;

        float elapsed = 0f;
        processingSlider.value = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            processingSlider.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        processingSlider.value = 0f; // Reset after completion
    }

    private void OnItemDelivered(ItemData deliveredItem, int amount)
    {
        if (deliveredItem != item) return;

        // If no more pending purchases, hide processing panel
        if (TransactionManager.Instance.GetQueueCount(item) <= 0)
        {
            if (processingPanel != null)
                processingPanel.SetActive(false);

            if (processingSlider != null)
                processingSlider.value = 0f;

            // Unsubscribe when done
            TransactionManager.Instance.OnItemDelivered -= OnItemDelivered;
        }

        Debug.Log($"Delivered {amount}x {item.itemName}");
    }
}
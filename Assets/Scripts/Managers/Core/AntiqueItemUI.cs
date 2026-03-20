using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Handles the UI for a single antique item, including queued purchases and delivery slider.
/// </summary>
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
    [SerializeField] private Slider processingSlider;
    [SerializeField] private TMP_Text queueCountText;

    private ItemData item;
    private Coroutine sliderCoroutine;

    /// <summary>
    /// Initializes the UI with item data.
    /// </summary>
    public void Setup(ItemData itemData)
    {
        item = itemData;
        if (item == null) return;

        icon.sprite = item.icon;
        nameText.text = item.itemName;
        eraText.text = item.era.ToString();

        // Safe button subscription
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);

        if (processingSlider != null)
            processingSlider.value = 0f;

        if (processingPanel != null)
            processingPanel.SetActive(false);

        // Safe event subscription
        TransactionManager.Instance.OnItemDelivered -= OnItemDelivered;
        TransactionManager.Instance.OnItemDelivered += OnItemDelivered;
    }

    /// <summary>
    /// Handles button click to buy item.
    /// </summary>
    private void BuyItem()
    {
        if (item == null) return;

        int cost = item.ObolValue;
        if (!CurrencyManager.Instance.HasEnough(cost))
        {
            Debug.LogWarning($"Not enough obols to buy {item.itemName}");
            return;
        }

        if (processingPanel != null)
            processingPanel.SetActive(true);

        TransactionManager.Instance.TryBuy(item, 1);

        if (sliderCoroutine == null)
            sliderCoroutine = StartCoroutine(ProcessSliderQueue());
    }

    /// <summary>
    /// Sequentially updates slider for each queued purchase.
    /// </summary>
    private IEnumerator ProcessSliderQueue()
    {
        while (TransactionManager.Instance.GetQueueCount(item) > 0)
        {
            float deliveryTime = Mathf.Max(item.deliveryTime, 0.1f);
            float elapsed = 0f;

            if (processingSlider != null)
                processingSlider.value = 0f;

            while (elapsed < deliveryTime)
            {
                elapsed += Time.deltaTime;
                if (processingSlider != null)
                    processingSlider.value = Mathf.Clamp01(elapsed / deliveryTime);
                yield return null;
            }

            if (processingSlider != null)
                processingSlider.value = 1f;

            yield return null;
        }

        // Reset UI
        if (processingPanel != null)
            processingPanel.SetActive(false);

        if (processingSlider != null)
            processingSlider.value = 0f;

        sliderCoroutine = null;
    }

    private void OnItemDelivered(ItemData deliveredItem, int amount)
    {
        if (deliveredItem != item) return;
        Debug.Log($"Delivered {amount}x {item.itemName}");
    }

    private void Update()
    {
        if (item == null) return;

        stockText.text = InventoryManager.Instance.GetStock(item).ToString();

        int pending = TransactionManager.Instance.GetQueueCount(item);
        queueCountText.text = pending > 0 ? pending.ToString() : "";
    }
}
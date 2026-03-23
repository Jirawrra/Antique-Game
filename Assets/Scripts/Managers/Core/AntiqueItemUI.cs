using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Displays a single antique item UI, handles visuals only.
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

    public void Setup(ItemData itemData)
    {
        item = itemData;
        if (item == null) return;

        icon.sprite = item.icon;
        nameText.text = item.itemName;
        eraText.text = item.era.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);

        // Reset visuals
        processingSlider.value = 0f;
        processingPanel.SetActive(false);
        queueCountText.text = "";
    }

    private void OnEnable()
    {
        if (TransactionManager.Instance != null)
            TransactionManager.Instance.OnItemDelivered += OnItemDelivered;
    }

    private void OnDisable()
    {
        if (TransactionManager.Instance != null)
            TransactionManager.Instance.OnItemDelivered -= OnItemDelivered;
    }

    private void BuyItem()
    {
        if (item == null) return;

        TransactionManager.Instance.TryBuy(item);

        // Immediate UI feedback
        UpdateQueueText();


    }

    private void Update()
    {
        if (item == null) return;

        //  Stock always accurate
        stockText.text = InventoryManager.Instance.GetStock(item).ToString();

        int queueCount = TransactionManager.Instance.GetQueueCount(item);

        //  Queue text
        queueCountText.text = queueCount > 0 ? queueCount.ToString() : "";

        if (queueCount > 0)
        {
            processingPanel.SetActive(true);

            //  Get REAL progress from TransactionManager
            float progress = TransactionManager.Instance.GetProgress(item);

            processingSlider.value = progress;
        }
        else
        {
            processingPanel.SetActive(false);
            processingSlider.value = 0f;
        }
    }

    private void UpdateQueueText()
    {
        int count = TransactionManager.Instance.GetQueueCount(item);
        queueCountText.text = count > 0 ? count.ToString() : "";
    }

    private void OnItemDelivered(ItemData deliveredItem, int amount)
    {
        if (deliveredItem != item) return;

        // Optional: feedback (sound, animation, etc.)
        Debug.Log($"Delivered {amount}x {item.itemName}");
    }
}
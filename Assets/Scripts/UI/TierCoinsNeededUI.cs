using UnityEngine;
using TMPro;

public class TierCoinsNeededUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nextTierText;

    private void Start()
    {
        if (GameMaster.Instance == null || nextTierText == null) return;

        // Subscribe immediately to tier changes
        GameMaster.Instance.OnTierChanged += UpdateTierText;

        // Display current tier's coins needed immediately
        UpdateTierText(GameMaster.Instance.CurrentTier);
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (GameMaster.Instance != null)
            GameMaster.Instance.OnTierChanged -= UpdateTierText;
    }

    // Matches Action<TierData> delegate
    private void UpdateTierText(TierData tier)
    {
        if (tier == null || nextTierText == null) return;

        nextTierText.text = $"<jump>{tier.coinsNeededToUnlockNextTier}</jump>";
    }
}
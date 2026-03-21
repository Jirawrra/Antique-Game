using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers.Core
{
    public class GameMaster : MonoBehaviour
    {
        public static GameMaster Instance { get; private set; }

        [Header("Tier")]
        [SerializeField] private TierData[] tiers;
        public int CurrentTierIndex { get; private set; }
        public TierData CurrentTier => tiers[CurrentTierIndex];
        public int CurrentTierLevel => CurrentTier.tierLevel; // Display tiers starting from 1 instead of 0
        public event Action<TierData> OnTierChanged;

        private void Start()
        {
            ApplyTier();
            CurrencyManager.Instance.OnObolsChanged += HandleCurrencyChanged;
        }
        private void OnDisable()
        {
            CurrencyManager.Instance.OnObolsChanged -= HandleCurrencyChanged;
        }

        private void Awake()
        {
            // Standard Singleton pattern
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Update()
        {
            // DEBUG: Press T to increase tier
            if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
            {

                IncreaseTier();
            }
        }


        public void IncreaseTier() // Call this when you want to move to the next tier
        {
            if (CurrentTierIndex >= tiers.Length - 1)
                return;

            CurrentTierIndex++;
            ApplyTier();
        }

        private void ApplyTier() // This method applies the current tier's settings to the game
        {
            OnTierChanged?.Invoke(CurrentTier);
            Debug.Log($"Tier changed to Tier {CurrentTierLevel}");
        }

        private void HandleCurrencyChanged(int newBalance)
        {
            if (CurrentTierIndex >= tiers.Length - 1)
                return;

            int requiredCoins = CurrentTier.coinsNeededToUnlockNextTier;

            if (newBalance >= requiredCoins)
            {
                IncreaseTier();
            }
        }

    }
}

using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
public class GameMaster : MonoBehaviour
{

    [Header("Tier")]
    [SerializeField] private TierData[] tiers;
    public int CurrentTierIndex { get; private set; }
    public TierData CurrentTier => tiers[CurrentTierIndex];
    public int CurrentTierLevel => CurrentTier.tierLevel + 1; // Display tiers starting from 1 instead of 0
    public event Action<TierData> OnTierChanged;


    private void Start()
    {
        ApplyTier();
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




}

using System;
using UnityEngine;
using UnityEngine.InputSystem; // for Keyboard.current

public class CurrencyManager : MonoBehaviour
{
    // Player currency
    [SerializeField] private int obols;   // Standard
    [SerializeField] private int drachma; // Premium

    // Events to notify UI
    public event Action<int> OnObolsChanged;
    public event Action<int> OnDrachmaChanged;

    // Properties to auto-update UI even if changed in Inspector
    public int Obols
    {
        get => obols;
        set
        {
            obols = value;
            OnObolsChanged?.Invoke(obols);
        }
    }

    public int Drachma
    {
        get => drachma;
        set
        {
            drachma = value;
            OnDrachmaChanged?.Invoke(drachma);
        }
    }

    private void Start()
    {
        // Make sure UI reflects starting values
        OnObolsChanged?.Invoke(obols);
        OnDrachmaChanged?.Invoke(drachma);
    }

    private void Update()
    {
        // Debug keys to add currency
        if (Keyboard.current != null)
        {
            if (Keyboard.current.oKey.wasPressedThisFrame)
                AddObols(10);

            if (Keyboard.current.dKey.wasPressedThisFrame)
                AddDrachma(5);
        }
    }

    // Public methods to modify currency
    public void AddObols(int amount) => Obols += amount;
    public void AddDrachma(int amount) => Drachma += amount;

    public bool SpendObols(int amount)
    {
        if (Obols >= amount)
        {
            Obols -= amount;
            return true;
        }
        return false;
    }

    public bool SpendDrachma(int amount)
    {
        if (Drachma >= amount)
        {
            Drachma -= amount;
            return true;
        }
        return false;
    }
}
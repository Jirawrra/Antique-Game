using Newtonsoft.Json.Bson;
using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{  
    // Player currency
    public int obols; // Standard
    public int drachma; // Premium

    public event Action<int> OnObolsChanged;
    public event Action<int> OnDrachmaChanged;

    private void Start()
    {
        OnObolsChanged?.Invoke(obols);
        OnDrachmaChanged?.Invoke(drachma);
    }

    public void AddObols(int amount)
    {
        obols += amount;
        OnObolsChanged?.Invoke(obols);
        Debug.Log("Obols : " + obols);
    }

    public void AddDrachma(int amount)
    {
        drachma += amount;
        OnDrachmaChanged?.Invoke(drachma);
        Debug.Log("Obols : " + obols);
    }

    public bool SpendObols(int amount)
    {
        if ( obols >= amount)
        {
            obols -= amount;
            OnObolsChanged?.Invoke(obols);
            return true;
        }
        return false;
    }

    public bool SpendDrachma(int amount)
    {
        if (drachma >= amount)
        {
            drachma -= amount;
            OnDrachmaChanged?.Invoke(drachma);
            return true;
        }
        return false;
    }
}
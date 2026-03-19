using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] private int startingBalance = 500;
    private int balance;

    public event Action<int> OnObolsChanged;

    private void Awake()
    {
        Instance = this;
        balance = startingBalance;

    }

    private void Start()
    {
        OnObolsChanged?.Invoke(balance); //fires after UIManager has subscribed in its own Start
    }


    public int GetBalance() => balance;

    public bool HasEnough(int amount) => balance >= amount;

    public bool Spend(int amount)
    {
        if (!HasEnough(amount))
        {
            Debug.LogWarning("Insufficient funds.");
            return false;
        }

        balance -= amount;
        OnObolsChanged?.Invoke(balance);
        Debug.Log($"Spent {amount}. Remaining balance: {balance}");
        return true;
    }

    public void Earn(int amount)
    {
        balance += amount;
        OnObolsChanged?.Invoke(balance);
        Debug.Log($"Earned {amount}. New balance: {balance}");
    }

}
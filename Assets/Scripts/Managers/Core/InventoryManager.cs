using UnityEngine;
using System;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<ItemData, int> inventory = new();

    public event Action OnInventoryChanged; // correct event type

    public void AddItem(ItemData item)
    {
        if (!inventory.ContainsKey(item))
            inventory[item] = 1;
        else
            inventory[item]++;

        OnInventoryChanged?.Invoke(); // notify UI
    }

    public void RemoveItem(ItemData item)
    {
        if (!inventory.ContainsKey(item)) return;

        inventory[item]--;

        if (inventory[item] <= 0)
            inventory.Remove(item);

        OnInventoryChanged?.Invoke(); // notify UI
    }

    public bool HasItem(ItemData item)
    {
        return inventory.ContainsKey(item);
    }

    public int GetItemCount(ItemData item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }
}
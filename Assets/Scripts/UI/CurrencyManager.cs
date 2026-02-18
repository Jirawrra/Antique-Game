using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int obols; // Player currency

    public void AddObols(int Amount)
    {
        obols += Amount;
        Debug.Log("Obols : " + obols);
    }

    public bool SpendObols(int amount)
    {
        if ( obols >= amount)
        {
            obols -= amount;
            return true;
        }

        return false;
    }
}
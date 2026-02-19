using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public GameObject Oboltext;
    public GameObject Drachmatext;
    
    // Player currency
    public int obols; // Standard
    public int drachma; // Premium

    public void AddObols(int Amount)
    {
        obols += Amount;
        Debug.Log("Obols : " + obols);
        
        drachma += Amount;
        Debug.Log("Obols : " + obols);
    }

    public bool SpendObols(int amount)
    {
        if ( obols >= amount)
        {
            obols -= amount;
            return true;
        }
        
        if ( drachma >= amount)
        {
            drachma -= amount;
            return true;
        }

        return false;
    }
}
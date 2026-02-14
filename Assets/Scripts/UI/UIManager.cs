using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //public Image UpgradeImage;
     public float duration = 0.25f;
    private RectTransform rect;

    public GameObject NotificationPanel;



    private void Start()
    {
        rect = GetComponent<RectTransform>();
        
    }

    public void Upgrades()
    {
        Debug.Log ("Show Upgrades UI");

    }

    public void Antique()
    {
            
        Debug.Log ("Show Antique UI");
    }
    public void Yes()
    {
        Debug.Log ("Yes");
        NotificationPanel.SetActive(false);
        
    }

    // IEnumerator Animate(Vector2 target)
    // {
    //     Vector2 start = rect.anchoredPosition;
    //     float time = 0f;

    //     while (time < duration)
    //     {
    //         time += Time.deltaTime;
    //         float t = time / duration;
    //         rect.anchoredPosition = Vector2.Lerp(start, target, t);
    //         yield return null;
    //     }

    //     rect.anchoredPosition = target;
    // }
    
    

 
}

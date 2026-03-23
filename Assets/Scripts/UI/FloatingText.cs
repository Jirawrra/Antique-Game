using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float duration = 1f;

    private CanvasGroup canvasGroup;
    private float timer;

    public void Setup(string message)
    {
        text.text = message;

        canvasGroup = GetComponent<CanvasGroup>();
        timer = duration;
    }

    private void Update()
    {
        // Move up
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Fade out
        timer -= Time.deltaTime;
        if (canvasGroup != null)
            canvasGroup.alpha = timer / duration;

        if (timer <= 0f)
            Destroy(gameObject);
    }
}
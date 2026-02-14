using UnityEngine;
using System;
using System.Collections;

public class PopOutAnimation : MonoBehaviour
{
    
    [Header("Animation Settings")]
    public float duration = 0.25f;

    [Tooltip("Easing curve for the animation (0=start, 1=end)")]
    public AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine animationCoroutine;

    void OnEnable()
    {
        UIManager.OnNotificationClosed += PlayCloseAnimation;
    }

    void OnDisable()
    {
        UIManager.OnNotificationClosed -= PlayCloseAnimation;
    }

    void PlayCloseAnimation()
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(AnimateClose());
    }

    IEnumerator AnimateClose()
    {
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.zero; // shrink to disappear
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            // Use easing curve
            float easedT = easingCurve.Evaluate(t);

            transform.localScale = Vector3.Lerp(startScale, targetScale, easedT);

            yield return null;
        }

        transform.localScale = targetScale;

        // Optionally disable panel after animation
        gameObject.SetActive(false);
    }
}

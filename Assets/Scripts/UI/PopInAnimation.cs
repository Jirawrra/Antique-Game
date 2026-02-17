using UnityEngine;
using System;
using System.Collections;

public class PopInAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float duration = 0.25f;
    public AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine animationCoroutine;

    void OnEnable()
    {
        UIManager.OnNotificationOpened += PlayOpenAnimation;
    }

    void OnDisable()
    {
        UIManager.OnNotificationOpened -= PlayOpenAnimation;
    }

    public void PlayOpenAnimation()
    {
        // Stop any running close animation
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        // Force enable panel before opening
        gameObject.SetActive(true);

        // Start the pop-in animation
        animationCoroutine = StartCoroutine(AnimateOpen());
    }

    IEnumerator AnimateOpen()
    {
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        float time = 0f;
        transform.localScale = startScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            float easedT = easingCurve.Evaluate(t);

            transform.localScale =
                Vector3.Lerp(startScale, targetScale, easedT);

            yield return null;
        }

        transform.localScale = targetScale;
    }

}

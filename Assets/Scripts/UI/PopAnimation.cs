using UnityEngine;
using System.Collections;

public class PopAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float duration = 0.25f;
    public AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine animationCoroutine;

    void OnEnable()
    {

        UIManager.OnNotificationOpened += PlayOpenAnimation;
        UIManager.OnNotificationClosed += PlayCloseAnimation;
    }

    void OnDisable()
    {
        UIManager.OnNotificationOpened -= PlayOpenAnimation;
        UIManager.OnNotificationClosed -= PlayCloseAnimation;
    }

    private void PlayOpenAnimation()
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        // Make sure panel is active
        gameObject.SetActive(true);

        animationCoroutine = StartCoroutine(Animate(Vector3.zero, Vector3.one));
    }

    private void PlayCloseAnimation()
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(Animate(transform.localScale, Vector3.zero, true));
    }

    private IEnumerator Animate(Vector3 from, Vector3 to, bool disableOnEnd = false)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            transform.localScale = Vector3.Lerp(from, to, easingCurve.Evaluate(t));
            yield return null;
        }

        transform.localScale = to;

        if (disableOnEnd)
            gameObject.SetActive(false);
    }
}

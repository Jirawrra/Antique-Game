using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    public float animationDuration = 0.3f;
    public Vector3 openScale = Vector3.one;
    public Vector3 closedScale = Vector3.zero;

    [Header("Easing Settings")]
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private CanvasGroup canvasGroup;
    private Coroutine currentAnimation;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        transform.localScale = closedScale;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        AnimateOpen();
    }

    public void AnimateOpen()
    {
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        gameObject.SetActive(true);
        currentAnimation = StartCoroutine(Animate(0f, 1f, closedScale, openScale));
    }

    public void AnimateClose()
    {
        if (currentAnimation != null) StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(Animate(1f, 0f, transform.localScale, closedScale, true));
    }

    private IEnumerator Animate(float startAlpha, float endAlpha, Vector3 startScale, Vector3 endScale, bool deactivateOnEnd = false)
    {
        float time = 0f;
        while (time < animationDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / animationDuration);
            float easedT = easeCurve.Evaluate(t); // Apply easing curve

            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, easedT);
            transform.localScale = Vector3.Lerp(startScale, endScale, easedT);

            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        transform.localScale = endScale;

        if (deactivateOnEnd)
            gameObject.SetActive(false);
    }

    public void ToggleUI()
    {
        if (canvasGroup.alpha > 0.5f)
            AnimateClose();
        else
            AnimateOpen();
    }
}
using UnityEngine;
using System.Collections;

public class PanelSlideMove : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private float moveAmount = 600f;
    [SerializeField] private float duration = 0.35f;
    [SerializeField]
    private AnimationCurve easeCurve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector2 downPos;
    private Vector2 upPos;
    private bool isUp;
    private Coroutine routine;

    private void Awake()
    {
        downPos = panel.anchoredPosition;
        upPos = downPos + Vector2.up * moveAmount;
        isUp = false;
    }

    public bool IsUp => isUp;

    public void SlideUp()
    {
        if (isUp) return;
        Move(upPos);
        isUp = true;
    }

    public void SlideDown()
    {
        if (!isUp) return;
        Move(downPos);
        isUp = false;
    }

    private void Move(Vector2 target)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(SmoothMove(target));
    }

    private IEnumerator SmoothMove(Vector2 target)
    {
        Vector2 start = panel.anchoredPosition;
        float t = 0f;

        while (t < duration)
        {
            float p = t / duration;
            panel.anchoredPosition =
                Vector2.Lerp(start, target, easeCurve.Evaluate(p));
            t += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = target;
    }
}

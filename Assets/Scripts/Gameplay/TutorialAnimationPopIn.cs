using System.Collections;
using UnityEngine;

public class TutorialAnimationPopIn : MonoBehaviour
{
    [Header("Slide Settings")]
    public float offScreenOffsetX = -20f;   // How far left off-screen to start
    public float slideDuration = 0.6f;       // Time to reach target
    public float overshootAmount = 1.5f;     // How far past target it goes
    public float bounceBackDuration = 0.25f; // Time to settle back to target

    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;

        // Move sprite off-screen to the left
        transform.position = new Vector3(
            targetPosition.x + offScreenOffsetX,
            targetPosition.y,
            targetPosition.z
        );
    }

    public void PlayAnimation()
    {
        StartCoroutine(SlideIn());
    }

    IEnumerator SlideIn()
    {
        // --- Phase 1: Slide in from left to overshoot point ---
        Vector3 startPos = transform.position;
        Vector3 overshootPos = new Vector3(
            targetPosition.x + overshootAmount,
            targetPosition.y,
            targetPosition.z
        );

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            float easedT = EaseOutCubic(t);
            transform.position = Vector3.LerpUnclamped(startPos, overshootPos, easedT);
            yield return null;
        }

        // --- Phase 2: Bounce back to final position ---
        elapsed = 0f;
        while (elapsed < bounceBackDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / bounceBackDuration);
            float easedT = EaseOutQuad(t);
            transform.position = Vector3.Lerp(overshootPos, targetPosition, easedT);
            yield return null;
        }

        transform.position = targetPosition; // Snap to exact target
    }

    float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
    float EaseOutQuad(float t) => 1f - (1f - t) * (1f - t);
}
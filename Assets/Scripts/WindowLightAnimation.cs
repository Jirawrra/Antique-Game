using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

[RequireComponent(typeof(Light2D))]
public class WindowLightAnimation : MonoBehaviour
{

    [Header("Base")]
    public float baseIntensity = 1.2f;

    [Header("Occlusion")]
    public float occludedIntensity = 0.35f;
    public float fadeInTime = 0.35f;
    public float holdTime = 0.45f;
    public float fadeOutTime = 0.7f;

    [Header("Trigger Timing")]
    public float minDelay = 8f;
    public float maxDelay = 18f;

    private Light2D light2D;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
        light2D.intensity = baseIntensity;
        StartCoroutine(OcclusionLoop());
    }

    IEnumerator OcclusionLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            yield return StartCoroutine(OcclusionPass());
        }
    }

    IEnumerator OcclusionPass()
    {
        // Person enters the light
        yield return Fade(baseIntensity, occludedIntensity, fadeInTime);

        // Silhouette blocks the window
        yield return new WaitForSeconds(holdTime);

        // Person exits the light
        yield return Fade(occludedIntensity, baseIntensity, fadeOutTime);
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            light2D.intensity = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        light2D.intensity = to;
    }
}

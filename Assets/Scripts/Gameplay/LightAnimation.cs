using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightAnimation : MonoBehaviour
{

    [Header("Base")]
    public float baseIntensity = 1.2f;

    [Header("Flicker")]
    public float flickerAmount = 0.6f;      // how strong the flicker is
    public float flickerSpeed = 18f;         // micro jitter speed

    [Header("Drops")]
    public float dropChance = 0.08f;         // chance per second
    public float dropIntensity = 0.15f;      // how dim it gets
    public float dropDuration = 0.12f;       // seconds

    private Light2D light2D;
    private float dropTimer;
    private bool dropping;
    private float noiseSeed;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
        noiseSeed = Random.Range(0f, 1000f);
    }

    void Update()
    {
        // Fast, nervous jitter
        float noise = Mathf.PerlinNoise(noiseSeed, Time.time * flickerSpeed);
        float jitter = (noise - 0.5f) * flickerAmount;

        // Random power drops
        if (!dropping && Random.value < dropChance * Time.deltaTime)
        {
            dropping = true;
            dropTimer = dropDuration;
        }

        if (dropping)
        {
            dropTimer -= Time.deltaTime;
            if (dropTimer <= 0f)
                dropping = false;
        }

        float targetIntensity = dropping
            ? dropIntensity
            : baseIntensity + jitter;

        light2D.intensity = Mathf.Max(0f, targetIntensity);
    }

}

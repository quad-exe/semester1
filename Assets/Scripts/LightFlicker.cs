using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    private Light2D light2D;

    [Header("Intensity Flicker Settings")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 5f;       // how quickly light transitions intensity
    public float changeFrequency = 0.1f;  // how often intensity target updates

    [Header("Color Flicker Settings")]
    public bool enableColorFlicker = true;
    public Color baseColor = new Color(1f, 0.75f, 0.5f); // warm orange
    public float colorVariation = 0.1f;  // how much color shifts

    private float targetIntensity;
    private float timer;
    private Color targetColor;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        targetIntensity = Random.Range(minIntensity, maxIntensity);
        targetColor = baseColor;
    }

    void Update()
    {
        if (light2D == null) return;

        // Smooth intensity flicker
        light2D.intensity = Mathf.Lerp(light2D.intensity, targetIntensity, Time.deltaTime * flickerSpeed);

        // Smooth color flicker
        if (enableColorFlicker)
        {
            light2D.color = Color.Lerp(light2D.color, targetColor, Time.deltaTime * flickerSpeed);
        }

        // Update targets occasionally
        timer += Time.deltaTime;
        if (timer >= changeFrequency)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);

            if (enableColorFlicker)
            {
                // Slight random offset from base color
                float r = Mathf.Clamp01(baseColor.r + Random.Range(-colorVariation, colorVariation));
                float g = Mathf.Clamp01(baseColor.g + Random.Range(-colorVariation, colorVariation));
                float b = Mathf.Clamp01(baseColor.b + Random.Range(-colorVariation, colorVariation));
                targetColor = new Color(r, g, b);
            }

            timer = 0f;
        }
    }
}


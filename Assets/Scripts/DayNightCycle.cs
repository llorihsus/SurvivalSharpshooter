using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Skyboxes")]
    public Material daySkybox;
    public Material eveningSkybox;
    public Material nightSkybox;

    [Header("Lighting")]
    public Light sun;
    public Gradient ambientColor;
    public Gradient sunColor;

    [Header("Time Settings")]
    public float fullDayLength = 120f; // seconds for full day/night cycle

    [Range(0f, 1f)]
    public float timeOfDay = 0.25f; // 0 = midnight, 0.25 = sunrise, 0.5 = noon, 0.75 = sunset

    void Update()
    {
        timeOfDay += Time.deltaTime / fullDayLength;

        if (timeOfDay >= 1f)
            timeOfDay = 0f;

        UpdateLighting();
        UpdateSkybox();
    }

    void UpdateLighting()
    {
        if (sun != null)
        {
            sun.transform.rotation = Quaternion.Euler((timeOfDay * 360f) - 90f, 170f, 0f);
            sun.color = sunColor.Evaluate(timeOfDay);
        }

        RenderSettings.ambientLight = ambientColor.Evaluate(timeOfDay);
    }

    void UpdateSkybox()
    {
        if (timeOfDay < 0.25f)
        {
            RenderSettings.skybox = nightSkybox;
        }
        else if (timeOfDay < 0.45f)
        {
            RenderSettings.skybox = eveningSkybox;
        }
        else if (timeOfDay < 0.70f)
        {
            RenderSettings.skybox = daySkybox;
        }
        else if (timeOfDay < 0.85f)
        {
            RenderSettings.skybox = eveningSkybox;
        }
        else
        {
            RenderSettings.skybox = nightSkybox;
        }

        DynamicGI.UpdateEnvironment();
    }
}
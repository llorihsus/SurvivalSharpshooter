using UnityEngine;
using TMPro;

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
    public float fullDayLength = 360f;

    [Range(0f, 1f)]
    public float timeOfDay = 0.25f;

    [Header("UI")]
    public TMP_Text timeText;

    [Header("Sun Damage")]
    public Health playerHealth;
    public Transform player;
    public float dayDamagePerSecond = 5f;

    void Update()
    {
        // Advance time
        timeOfDay += Time.deltaTime / fullDayLength;
        if (timeOfDay >= 1f)
            timeOfDay = 0f;

        UpdateLighting();
        UpdateSkybox();
        UpdateTimeUI();
        DamagePlayerInSunlight();
    }

    void UpdateLighting()
    {
        if (sun != null)
        {
            sun.transform.rotation = Quaternion.Euler((timeOfDay * 360f) - 90f, 170f, 0f);
            sun.color = sunColor.Evaluate(timeOfDay);
            sun.intensity = Mathf.Clamp01(Mathf.Sin(timeOfDay * Mathf.PI));
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

    void UpdateTimeUI()
    {
        if (timeText == null) return;

        float totalMinutes = timeOfDay * 24f * 60f;

        int hours = Mathf.FloorToInt(totalMinutes / 60f);
        int minutes = Mathf.RoundToInt(totalMinutes % 60f);

        string period = hours >= 12 ? "PM" : "AM";

        int displayHour = hours % 12;
        if (displayHour == 0)
            displayHour = 12;

        timeText.text = displayHour + ":" + minutes.ToString("00") + " " + period;
    }

    void DamagePlayerInSunlight()
    {
        if (playerHealth == null || player == null || sun == null) return;

        bool isDaytime = timeOfDay >= 0.45f && timeOfDay < 0.70f;
        if (!isDaytime) return;

        Vector3 rayStart = player.position + Vector3.up * 1.5f;
        Vector3 sunDirection = -sun.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(rayStart, sunDirection, out hit, 100f))
        {
            // If we hit something that is not the player, we are in shadow
            if (!hit.transform.IsChildOf(player))
            {
                return;
            }
        }

        // No obstruction, take damage
        float damage = dayDamagePerSecond * Time.deltaTime;

        PickupLogic pickupLogic = player.GetComponent<PickupLogic>();

        if (pickupLogic != null)
        {
            damage = pickupLogic.AbsorbSunDamage(damage);
        }

        if (damage > 0f)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
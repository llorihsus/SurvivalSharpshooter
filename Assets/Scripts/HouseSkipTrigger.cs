using UnityEngine;

public class HouseSkipTrigger : MonoBehaviour
{
    [SerializeField] private GameObject skipButton;

    private bool playerNearby = false;

    private void Start()
    {
        if (skipButton != null)
        {
            skipButton.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            DayNightCycle timeManager = FindAnyObjectByType<DayNightCycle>();

            if (timeManager != null && timeManager.timeOfDay < 0.6f)
            {
                if (skipButton != null)
                {
                    skipButton.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            if (skipButton != null)
            {
                skipButton.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SkipToSunset()
    {
        if (!playerNearby) return;

        DayNightCycle timeManager = FindAnyObjectByType<DayNightCycle>();

        if (timeManager != null)
        {
            // Only allow during daytime
            if (timeManager.timeOfDay < 0.6f)
            {
                timeManager.SkipToSunset();
            }
            else
            {
                Debug.Log("Too late to skip time");
                return;
            }
        }

        if (skipButton != null)
        {
            skipButton.SetActive(false);
        }
    }
}
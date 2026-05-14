using System;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class GameEvents
{
    public static Action OnWin;
}

public class Health : MonoBehaviour
{
    [SerializeField] private HealthData healthData; // ScriptableObject for health values
    [SerializeField] private Animator deathAnimator; // Reference to Animator 
    public bool isDead = false;
    public static int killCount = 0;
    public int winKillCount = 10; // Number of kills needed to win
    private float currentHealth;
    [SerializeField] private bool disableAfterDeath = false;
    [SerializeField] public Image healthBar;
    [SerializeField] public GameObject deathMenu;
    [SerializeField] public GameObject winMenu;
    [SerializeField] public TMP_Text killCountText; // kill count text

    private void Start()
    {
        if (healthData == null)
        {
            Debug.LogError(gameObject.name + " is missing HealthData!");
            enabled = false;
            return;
        }

        // Initialize health from ScriptableObject
        currentHealth = healthData.maxHealth;

        // If animator not assigned in Inspector, grab it automatically
        if (deathAnimator == null)
        {
            deathAnimator = GetComponent<Animator>();
        }
    }

    public void TakeDamage(float amount)
    {
        // Prevent taking damage after death
        if (isDead) return;

        // Reduce health
        currentHealth -= amount;
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / healthData.maxHealth;
        }

        Debug.Log(gameObject.name + " health: " + currentHealth);

        // If health reaches 0, die
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        Ghoul ghoul = GetComponent<Ghoul>();
        if (ghoul != null)
        {
            ghoul.PlayHurtAnimation();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(healthData.maxHealth, currentHealth + amount);
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / healthData.maxHealth;
        }
    }

    // Resets health when reused from object pool
    public void ResetHealthToMax()
    {
        currentHealth = healthData.maxHealth;

        if (healthBar != null)
        {
            healthBar.fillAmount = 1f;
        }

        isDead = false;

        // Reset animation state so zombie doesn't stay dead when reused
        if (deathAnimator != null)
        {
            deathAnimator.ResetTrigger("Die");
            deathAnimator.Play("Idle"); // Make sure this matches your idle state name
        }
    }

    // Handles death logic
    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " has died!");

        // Stop player movement
        FirstPersonController controller = GetComponent<FirstPersonController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        // Try to play death animation only if assigned
        if (deathAnimator != null)
        {
            deathAnimator.SetTrigger("Die");
        }

        // If this is the player, go to game over
        if (!disableAfterDeath)
        {
            StartCoroutine(PlayerDeathSequence());
        }

        // If this is a zombie, disable after delay for pooling
        if (disableAfterDeath)
        {
            StartCoroutine(DisableAfterDeathAnimation());
            killCount++;
            if (killCount >= winKillCount)
            {
                GameEvents.OnWin?.Invoke();
            }
            Debug.Log("Kill Count: " + killCount);
        }
    }

    void OnEnable()
    {
        GameEvents.OnWin += WinSequence;
    }

    // Waits for animation to finish, then disables object (for pooling)
    private IEnumerator DisableAfterDeathAnimation()
    {
        yield return new WaitForSeconds(healthData.deathDelay);

        //Do NOT destroy, just disable for object pooling
        gameObject.SetActive(false);
    }

    private IEnumerator PlayerDeathSequence()
    {
        yield return new WaitForSeconds(healthData.deathDelay);

        // later replace this with your game over UI
        if (deathMenu != null)
        {
            deathMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            killCount = 0; // reset kill count on player death
            Time.timeScale = 0f; // pause the game
        }
        Debug.Log("GAME OVER");
    }

    private void WinSequence()
    {
        // Show win menu
        if (winMenu != null)
        {
            TMP_Text killCountDisplay = killCountText.GetComponent<TMP_Text>();
            if (killCountDisplay != null)
            {
                killCountDisplay.text = "You killed " + killCount + " Enemies!";
            }
            winMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; // pause the game
            killCount = 0; // reset kill count after winning
        }
        Debug.Log("YOU WIN!");
    }
}
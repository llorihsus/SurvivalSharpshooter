using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Animator deathAnimator; // Reference to Animator 
    [SerializeField] private float deathDelay = 2f;   // How long to wait before disabling object (length of death animation)
    public bool isDead = false;
    private float currentHealth;
    [SerializeField] private bool disableAfterDeath = false;
    [SerializeField] public Image healthBar;

    private void Start()
    {
        currentHealth = maxHealth;

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
            healthBar.fillAmount = currentHealth / maxHealth;
        }
        Debug.Log(gameObject.name + " health: " + currentHealth);

        // If health reaches 0, die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    // Resets health when reused from object pool
    public void ResetHealthToMax()
    {
        currentHealth = maxHealth;
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
        }
    }

    // Waits for animation to finish, then disables object (for pooling)
    private IEnumerator DisableAfterDeathAnimation()
    {
        yield return new WaitForSeconds(deathDelay);

        //Do NOT destroy, just disable for object pooling
        gameObject.SetActive(false);
    }

    private IEnumerator PlayerDeathSequence()
    {
        yield return new WaitForSeconds(1.5f);

        // later replace this with your game over UI
        Debug.Log("GAME OVER");
    }
}

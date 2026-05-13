using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] private Animator deathAnimator;
    public bool isDead = false;
    float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ResetHealthToMax()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    void Die()
    {
        //timed destruction allows death animation to play before object is removed from scene
        deathAnimator.SetTrigger("Die");
        isDead = true;
        Debug.Log(gameObject.name + " has died!");
        if (gameObject.name.Contains("Zombie"))
        {
            gameObject.SetActive(false);
        }
    }
}

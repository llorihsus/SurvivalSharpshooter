using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private Animator zombieAnimator;
    [SerializeField] private EnemyData enemyData; // ScriptableObject for enemy stats

    FirstPersonController player;
    NavMeshAgent agent;
    private Health zombieHealth;
    private bool isAttacking = false;

    // Awake: grab components ON this GameObject
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start: grab references from OTHER GameObjects
    void Start()
    {
        player = FindFirstObjectByType<FirstPersonController>();
        zombieHealth = GetComponent<Health>();
    }

    // Update destination every frame so robot follows player
    void Update()
    {
        if (zombieHealth != null && zombieHealth.isDead)
        {
            agent.enabled = false; // Stop the NavMeshAgent so it doesn't slide
            return;
        }

        agent.SetDestination(player.transform.position);

        zombieAnimator.SetFloat("MoveSpeed", agent.velocity.magnitude); // Updates Walk/Idle animation

        if ((agent.remainingDistance <= agent.stoppingDistance) && !isAttacking)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    System.Collections.IEnumerator AttackPlayer()
    {
        // Stop the agent from sliding during the swing
        isAttacking = true;
        agent.isStopped = true;
        zombieAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(enemyData.attackDelay);
        if (zombieHealth != null && zombieHealth.isDead)
        {
            yield break; // Exit if the zombie died during the attack animation
        }

        // Check if player is still in range after the attack animation plays
        if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance + enemyData.attackRangeBonus)
        {
            player.GetComponent<Health>().TakeDamage(enemyData.attackDamage);
        }

        
        agent.isStopped = false; // Resume movement after the attack
        isAttacking = false;
    }
}
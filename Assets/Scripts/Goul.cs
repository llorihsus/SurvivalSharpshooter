using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
public class Goul : MonoBehaviour
{
    [SerializeField] private Animator goulAnimator;
    FirstPersonController player;
    NavMeshAgent agent;
    private Health goulHealth;
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
        goulHealth = GetComponent<Health>();
    }
    // Update destination every frame so robot follows player
    void Update()
    {
        if (goulHealth != null && goulHealth.isDead)
        {
            agent.enabled = false; // Stop the NavMeshAgent so it doesn't slide
            return; 
        }

        agent.SetDestination(player.transform.position); 

        goulAnimator.SetFloat("MoveSpeed", agent.velocity.magnitude); // Updates Walk/Idle animation

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
        goulAnimator.SetTrigger("Melee");
        yield return new WaitForSeconds(1.4f);

        if (goulHealth != null && goulHealth.isDead)
        {
            yield break; // Exit if the goul died during the attack animation
        }

        // Check if player is still in range after the attack animation plays
        if (Vector3.Distance(transform.position, player.transform.position) <= agent.stoppingDistance + 1f)
        {
            player.GetComponent<Health>().TakeDamage(20f);
        }
        
        agent.isStopped = false; // Resume movement after the attack
        isAttacking = false;
    }
}

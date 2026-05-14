using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Ghoul : MonoBehaviour
{
    [SerializeField] private Animator goulAnimator;
    [SerializeField] private EnemyData enemyData;

    private FirstPersonController player;
    private NavMeshAgent agent;
    private Health goulHealth;
    private bool isAttacking = false;
    private bool isHit = false;

    [SerializeField] private float hitSlowDuration = 0.4f;
    [SerializeField] private float hitSlowMultiplier = 0.25f;

    private float originalSpeed;
    private Coroutine hitSlowRoutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (goulAnimator == null)
        {
            goulAnimator = GetComponent<Animator>();
        }
    }

    private void Start()
    {
        player = FindFirstObjectByType<FirstPersonController>();
        goulHealth = GetComponent<Health>();
    }

    private void Update()
    {
        if (player == null || agent == null || goulAnimator == null)
            return;

        // stop everything if ghoul is dead
        if (goulHealth != null && goulHealth.isDead)
        {
            agent.isStopped = true;
            goulAnimator.SetFloat("MoveSpeed", 0f);
            return;
        }

        // chase player if not attacking
        if (!isAttacking)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);

            // update movement animation
            goulAnimator.SetFloat("MoveSpeed", agent.velocity.magnitude);

        }

        // if close enough to player, attack
        if (!agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance &&
            !isAttacking)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        // stop moving during attack
        agent.isStopped = true;

        // play attack animation
        goulAnimator.SetFloat("MoveSpeed", 0f);
        goulAnimator.SetTrigger("Melee");

        // wait for attack timing
        yield return new WaitForSeconds(enemyData.attackDelay);

        // damage player if still alive and close enough
        if (goulHealth == null || !goulHealth.isDead)
        {
            if (Vector3.Distance(transform.position, player.transform.position)
                <= agent.stoppingDistance + enemyData.attackRangeBonus)
            {
                Health playerHealth = player.GetComponent<Health>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(enemyData.attackDamage);
                }
            }
        }

        // resume chasing player
        if (goulHealth == null || !goulHealth.isDead)
        {
            agent.isStopped = false;
        }

        isAttacking = false;
    }

    public void PlayHurtAnimation()
    {
        if (goulAnimator != null)
        {
            goulAnimator.SetTrigger("Hit");
        }
    }
}
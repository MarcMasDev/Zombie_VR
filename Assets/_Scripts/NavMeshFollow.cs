using UnityEngine;
using UnityEngine.AI;

public class NavMeshFollow : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [Header("Chase Settings")]
    public float chaseRange = 10f;
    public float stopDistance = 1.5f;


    private Animator animator;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.stoppingDistance = stopDistance;
    }

    private void Update()
    {
        if (!agent.enabled) return;

        float distance = agent.remainingDistance;

        if (distance <= stopDistance && agent.hasPath)
        {
            agent.isStopped = true;
            agent.updateRotation = false;
            animator.SetBool("Attack", true);

            FacePlayer();
        }
        else if (Vector3.Distance(transform.position, player.position) <= chaseRange)
        {
            agent.isStopped = false;
            agent.updateRotation = true;
            agent.SetDestination(player.position);
            animator.SetBool("Attack", false);
        }
        else
        {
            agent.isStopped = true;
            agent.updateRotation = true;
            animator.SetBool("Attack", false);
        }

        animator.SetFloat("Velocity", GetVelocity());
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation, Time.deltaTime * 10f);
        }
    }

    private float GetVelocity()
    {
        if (agent.speed >= 1) return 1; return 0;
    }

}

using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform patrolRoute;

    [Header("Settings")]
    [SerializeField] private float patrolSpeed = 1.5f;
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float stopAtDistance = 0.5f;

    private NavMeshAgent agent;
    private Transform[] waypoints;
    private int currentPatrolPoint;
    private bool isWaiting;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        waypoints = new Transform[patrolRoute.childCount];
        for (int i = 0; i < patrolRoute.childCount; i++)
            waypoints[i] = patrolRoute.GetChild(i);
    }

    public void StartPatrol()
    {
        if (waypoints.Length == 0) return;
        agent.speed = patrolSpeed;
        agent.SetDestination(waypoints[currentPatrolPoint].position);
    }

    public void Patrol()
    {
        if (isWaiting) return;
        if (!agent.pathPending && agent.remainingDistance <= stopAtDistance)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(patrolWaitTime);

        agent.isStopped = false;
        GoToNextWaypoint();
        isWaiting = false;
    }

    public void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        currentPatrolPoint = (currentPatrolPoint + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentPatrolPoint].position);
        Debug.Log($"Enemy Going to waypoint {currentPatrolPoint}: {waypoints[currentPatrolPoint].position}");
    }

    public bool IsWaiting() => isWaiting;
}

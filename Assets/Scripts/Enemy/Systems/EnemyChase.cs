using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float chaseSpeed = 4f; // m/s
    [SerializeField] private float chaseStopDistance = 1.5f; // how close the enemy gets to the player
    [SerializeField] private float repathInterval = 0.2f; // seconds between destination updates
    [SerializeField] private float loseSightTime = 3f; // seconds out of sight before giving up the chase

    public event Action OnTargetLost;

    private NavMeshAgent agent;
    private float defaultStoppingDistance;
    private float repathTimer;
    private float lostTimer;
    private bool targetLost;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartChase()
    {
        defaultStoppingDistance = agent.stoppingDistance;
        agent.speed = chaseSpeed;
        agent.stoppingDistance = chaseStopDistance;
        agent.isStopped = false;
        repathTimer = 0f;
        lostTimer = 0f;
        targetLost = false;
    }

    public void Chase(Vector3 target, bool canSee)
    {
        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            agent.SetDestination(target);
            repathTimer = repathInterval;
        }

        if (canSee)
        {
            lostTimer = 0f;
            return;
        }

        lostTimer += Time.deltaTime;
        if (!targetLost && lostTimer >= loseSightTime)
        {
            targetLost = true;
            OnTargetLost?.Invoke();
        }
    }

    public void StopChase()
    {
        agent.ResetPath();
        agent.stoppingDistance = defaultStoppingDistance;
    }
}

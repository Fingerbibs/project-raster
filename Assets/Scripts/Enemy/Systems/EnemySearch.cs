using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemySearch : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float searchSpeed = 2.5f; // m/s
    [SerializeField] private float arriveDistance = 0.5f; // distance counted as reaching the last known position
    [SerializeField] private float searchDuration = 6f; // seconds spent looking around before giving up
    [SerializeField] private float lookAngle = 60f; // degrees either side of the arrival facing
    [SerializeField] private float lookSpeed = 1.5f; // sweep frequency

    public event Action OnSearchFinished;

    private NavMeshAgent agent;
    private bool hasArrived;
    private bool searchFinished;
    private float searchTimer;
    private float baseYaw;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartSearch(Vector3 lastKnownPosition)
    {
        agent.speed = searchSpeed;
        agent.isStopped = false;
        agent.SetDestination(lastKnownPosition);
        hasArrived = false;
        searchFinished = false;
        searchTimer = 0f;
    }

    public void Search()
    {
        if (!hasArrived)
        {
            if (agent.pathPending || agent.remainingDistance > arriveDistance) return;
            hasArrived = true;
            agent.ResetPath();
            baseYaw = transform.eulerAngles.y;
        }

        searchTimer += Time.deltaTime;
        float yaw = baseYaw + Mathf.Sin(searchTimer * lookSpeed) * lookAngle;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        if (!searchFinished && searchTimer >= searchDuration)
        {
            searchFinished = true;
            OnSearchFinished?.Invoke();
        }
    }

    public void StopSearch() => agent.ResetPath();
}

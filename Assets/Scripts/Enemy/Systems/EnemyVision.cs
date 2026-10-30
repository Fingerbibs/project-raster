using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyVision : MonoBehaviour
{
    [Header("Cone")]
    [SerializeField] private float viewDistance = 10f; // metres
    [SerializeField] private float viewAngle = 90f; // full cone angle, degrees
    [SerializeField] private float eyeHeight = 1.6f; // eye offset above pivot

    [Header("Detection")]
    [SerializeField] private float detectTime = 0.3f; // seconds of continuous sight before spotted
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;

    public event Action OnPlayerSpotted;

    public Vector3 LastKnownPlayerPosition { get; private set; }

    private readonly Collider[] overlapBuffer = new Collider[4];
    private float detectTimer;
    private bool canSeePlayer;
    private bool hasSpotted;

    private Vector3 EyePosition => transform.position + Vector3.up * eyeHeight;

    private void Update()
    {
        canSeePlayer = ScanForPlayer();

        if (!canSeePlayer)
        {
            detectTimer = 0f;
            hasSpotted = false;
            return;
        }

        detectTimer += Time.deltaTime;
        if (!hasSpotted && detectTimer >= detectTime)
        {
            hasSpotted = true;
            OnPlayerSpotted?.Invoke();
        }
    }

    private bool ScanForPlayer()
    {
        Vector3 eye = EyePosition;
        int count = Physics.OverlapSphereNonAlloc(eye, viewDistance, overlapBuffer, playerMask);

        for (int i = 0; i < count; i++)
        {
            Vector3 target = overlapBuffer[i].bounds.center;
            if (Vector3.Angle(transform.forward, target - eye) > viewAngle * 0.5f) continue;
            if (Physics.Linecast(eye, target, obstacleMask)) continue;

            LastKnownPlayerPosition = target;
            return true;
        }
        return false;
    }

    public bool CanSeePlayer() => canSeePlayer;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 eye = EyePosition;
        Color color = canSeePlayer ? Color.red : Color.yellow;

        Vector3 left = Quaternion.AngleAxis(-viewAngle * 0.5f, Vector3.up) * transform.forward;
        Vector3 right = Quaternion.AngleAxis(viewAngle * 0.5f, Vector3.up) * transform.forward;

        Handles.color = new Color(color.r, color.g, color.b, 0.15f);
        Handles.DrawSolidArc(eye, Vector3.up, left, viewAngle, viewDistance);

        Gizmos.color = color;
        Gizmos.DrawLine(eye, eye + left * viewDistance);
        Gizmos.DrawLine(eye, eye + right * viewDistance);
        if (canSeePlayer) Gizmos.DrawLine(eye, LastKnownPlayerPosition);
    }
#endif
}

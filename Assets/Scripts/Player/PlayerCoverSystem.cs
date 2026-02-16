using UnityEngine;

public class PlayerCoverSystem : MonoBehaviour
{
    [Header("Cover Settings")]
    [SerializeField] private float coverMoveSpeed = 2f;
    [SerializeField] private float enterDotThreshold = 0.7f;
    [SerializeField] private float wallCheckDistance = 1.5f;
    [SerializeField] private float edgeCheckOffset = 0.3f;

    private PlayerMovement playerMovement;
    private CharacterController characterController;

    private Vector3 coverNormal;
    private Vector3 coverRight;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!IsInCover()) return;

        Vector3 moveDir = GetNormalizedMovement();
        if (moveDir == Vector3.zero) return;

        // Exit if pushing away from wall
        if (Vector3.Dot(moveDir, coverNormal) > enterDotThreshold)
        {
            ExitCover();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Cover")) return;
        if (IsInCover()) return;

        Vector3 moveDir = GetNormalizedMovement();
        if (moveDir == Vector3.zero) return;

        Vector3 wallNormal = hit.normal;
        wallNormal.y = 0f;
        wallNormal.Normalize();

        if (Vector3.Dot(moveDir, -wallNormal) > enterDotThreshold)
        {
            EnterCover(wallNormal);
        }
    }

    private void EnterCover(Vector3 wallNormal)
    {
        playerMovement.currentState = MovementState.Cover;

        coverNormal = wallNormal;
        coverRight = Vector3.Cross(coverNormal, Vector3.up).normalized;

        SnapToWall();
    }

    private void ExitCover()
    {
        playerMovement.currentState = MovementState.Free;
        coverNormal = Vector3.zero;
    }

    public void HandleCoverMovement()
    {
        if (!IsInCover()) return;

        Vector3 moveDir = GetNormalizedMovement();
        if (moveDir == Vector3.zero) return;

        float moveAmount = Vector3.Dot(moveDir, coverRight);
        Vector3 move = coverRight * moveAmount * coverMoveSpeed * Time.deltaTime;

        characterController.Move(move);

        CheckIfStillInCover(moveAmount);
    }

    private void CheckIfStillInCover(float moveAmount)
    {
        // Offset ray toward movement direction
        Vector3 origin = transform.position + coverRight * Mathf.Sign(moveAmount) * edgeCheckOffset;

        if (!Physics.Raycast(origin, -coverNormal, wallCheckDistance))
        {
            ExitCover();
        }
    }

    private void SnapToWall()
    {
        if (!Physics.Raycast(transform.position, -coverNormal, out RaycastHit hit, 2f))
            return;

        characterController.enabled = false;

        float playerRadius = characterController.radius;
        Vector3 snapPosition = hit.point + coverNormal * playerRadius;
        transform.position = snapPosition;

        Vector3 lookDirection = coverNormal;
        lookDirection.y = 0f;

        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection);

        characterController.enabled = true;
    }

    private Vector3 GetNormalizedMovement()
    {
        Vector3 moveDir = playerMovement.GetPlayerMovementDirection();
        if (moveDir.magnitude < 0.1f) return Vector3.zero;
        return moveDir.normalized;
    }

    public bool IsInCover()
        => playerMovement.currentState == MovementState.Cover;
}
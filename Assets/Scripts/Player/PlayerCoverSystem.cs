using UnityEngine;

public class PlayerCoverSystem : MonoBehaviour
{
    [Header("Cover Settings")]
    [SerializeField] private float coverMoveSpeed = 2f;
    [SerializeField] private float enterDotThreshold = 0.7f;
    [SerializeField] private float wallCheckDistance = 1.5f;
    [SerializeField] private float edgeCheckOffset = 0.3f;
    [SerializeField] private float nearEdgeDistance = 2f;

    private PlayerMovement playerMovement;
    private CharacterController characterController;

    private Vector3 coverNormal;
    private Vector3 coverRight;
    private float lastMoveDirection;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        characterController = GetComponent<CharacterController>();
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
            EnterCover(wallNormal);
    }

    #region HANDLE COVER
    public void HandleCoverMovement()
    {
        if (!IsInCover()) return;

        Vector3 moveDir = GetNormalizedMovement();
        if (moveDir == Vector3.zero) return;

        if (Vector3.Dot(moveDir, coverNormal) > enterDotThreshold)
        {
            ExitCover();
            return;
        }

        float moveAmount = Vector3.Dot(moveDir, coverRight);

        if (Mathf.Abs(moveAmount) > 0.1f)
            lastMoveDirection = Mathf.Sign(moveAmount);

        characterController.Move(coverRight * moveAmount * coverMoveSpeed * Time.deltaTime);
        CheckForEdge(moveAmount);
    }

    private void EnterCover(Vector3 wallNormal)
    {
        playerMovement.SetState(MovementState.Cover);
        coverNormal = wallNormal;
        coverRight = Vector3.Cross(coverNormal, Vector3.up).normalized;
        SnapToWall();
    }

    private void SnapToWall()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * (characterController.height / 2f);

        if (!Physics.Raycast(rayOrigin, -coverNormal, out RaycastHit hit, 2f))
            return;

        characterController.enabled = false;

        Vector3 snapPosition = hit.point + coverNormal * characterController.radius;
        snapPosition.y = transform.position.y;
        transform.position = snapPosition;

        Vector3 lookDirection = new Vector3(coverNormal.x, 0f, coverNormal.z);
        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection);

        characterController.enabled = true;
    }

    private void ExitCover()
    {
        playerMovement.SetState(MovementState.Free);
        coverNormal = Vector3.zero;
    }

    private void CheckForEdge(float moveAmount)
    {
        float offset = characterController.radius + edgeCheckOffset;
        if (IsWallSideOpen(coverRight * Mathf.Sign(moveAmount), offset))
            ExitCover();
    }
    #endregion

    #region HELPER
    private bool IsWallSideOpen(Vector3 direction, float distance)
    {
        Vector3 origin = transform.position + direction * distance;
        return !Physics.Raycast(origin, -coverNormal, wallCheckDistance);
    }

    public bool IsNearEdge() => IsRightSideOpen() || IsLeftSideOpen();

    public bool IsRightSideOpen() => IsInCover() && IsWallSideOpen(coverRight, nearEdgeDistance);
    public bool IsLeftSideOpen()  => IsInCover() && IsWallSideOpen(-coverRight, nearEdgeDistance);

    private Vector3 GetNormalizedMovement()
    {
        Vector3 moveDir = playerMovement.GetPlayerMovementDirection();
        return moveDir.magnitude < 0.1f ? Vector3.zero : moveDir.normalized;
    }

    public float GetLastMoveDirection() => lastMoveDirection;

    public bool IsInCover() => playerMovement.GetState() == MovementState.Cover;
    
    #endregion
}
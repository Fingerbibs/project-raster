using UnityEngine;
using System;

public class CoverMovement : MovementType
{
    [Header("Cover Settings")]
    [SerializeField] private float enterThreshold = 0.7f; // Threshold for angle entry(precentage aligned to wall)
    [SerializeField] private float wallCheckDistance = 1.5f; // How far from the wall for coverEntering
    [SerializeField] private float exitOffset = 0.3f; // How close the player can be the the edge of the wall before exiting
    [SerializeField] private float nearEdgeDistance = 2f; // How close to the edge of the wall before considered "near" edge

    public event Action OnCoverEntered;
    public event Action OnCoverExited;

    private Vector3 coverNormal; // Vector perpendicular to wall length
    private Vector3 coverRight; // Vector parallel to wall length
    private float lastMoveDirection;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.CompareTag("Cover")) return;
        if (IsInCover()) return;
        Vector3 moveDir = playerController.GetNormalizedMovement();
        if (moveDir == Vector3.zero) return;

        // Grab the normal vector of the cover and compare it to the player' move direction,
        // If player is more than (%enterThreshold) aligned with the cover, EnterCover
        coverNormal = hit.normal;
        coverNormal.y = 0f;
        coverNormal.Normalize();
        if (Vector3.Dot(moveDir, -coverNormal) > enterThreshold)
            EnterCover();
    }

    /// <summary>
    /// Once in cover, the player can move side to side
    /// </summary>
    public override void HandleMovement()
    {
        if (!IsInCover()) return;

        Vector3 moveDir = playerController.GetNormalizedMovement();
        if (moveDir == Vector3.zero) return;

        if (Vector3.Dot(moveDir, coverNormal) > enterThreshold)
        {
            OnCoverExited?.Invoke();
            return;
        }

        float moveAmount = Vector3.Dot(moveDir, coverRight);

        if (Mathf.Abs(moveAmount) > 0.1f)
            lastMoveDirection = Mathf.Sign(moveAmount);

        characterController.Move(coverRight * moveAmount * moveSpeed * Time.deltaTime);
        CheckForEdge(moveAmount);
    }

    /// <summary>
    /// Set the vector parallel to the path of the wall and snap the player to the wall
    /// </summary>
    private void EnterCover()
    {
        OnCoverEntered?.Invoke();
        coverRight = Vector3.Cross(coverNormal, Vector3.up).normalized;
        SnapToWall();
    }

    private void SnapToWall()
    {
        // If player isn't within 2m of the wall, do NOT snap
        Vector3 rayOrigin = transform.position + Vector3.up * (characterController.height / 2f);
        if (!Physics.Raycast(rayOrigin, -coverNormal, out RaycastHit hit, wallCheckDistance))
            return;

        // Disable the CharacterController and snap the player to the Raycast hit point(the wall)
        characterController.enabled = false;

        Vector3 snapPosition = hit.point + coverNormal * characterController.radius;
        snapPosition.y = transform.position.y;
        transform.position = snapPosition;

        Vector3 lookDirection = new Vector3(coverNormal.x, 0f, coverNormal.z);
        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection);

        characterController.enabled = true;
    }

    private void CheckForEdge(float moveAmount)
    {
        float offset = characterController.radius + exitOffset;
        if (IsWallSideOpen(coverRight * Mathf.Sign(moveAmount), offset))
            OnCoverExited?.Invoke();
    }

    private bool IsWallSideOpen(Vector3 direction, float distance)
    {
        Vector3 origin = transform.position + direction * distance;
        return !Physics.Raycast(origin, -coverNormal, wallCheckDistance);
    }

    /// <summary>
    /// Called by CoverState.Exit() to clean up cover data.
    /// </summary>
    public void ResetCover()
    {
        coverNormal = Vector3.zero;
        coverRight = Vector3.zero;
    }

    public bool IsNearEdge()      => IsRightSideOpen() || IsLeftSideOpen();
    public bool IsRightSideOpen() => IsInCover() && IsWallSideOpen(coverRight, nearEdgeDistance);
    public bool IsLeftSideOpen()  => IsInCover() && IsWallSideOpen(-coverRight, nearEdgeDistance);
    public bool IsInCover() => playerController.GetState() == MovementState.Cover;
    public float GetLastMoveDirection() => lastMoveDirection;
}
using UnityEngine;

public  class FreeMovement : MovementType
{
    [SerializeField] public float rotationSpeed = 8f;

    public override void HandleMovement()
    {
        Vector3 moveDirection = playerController.GetMovementDirection();

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            RotateTowards(moveDirection);
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void RotateTowards(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        playerController.transform.rotation = Quaternion.Slerp(
            playerController.transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
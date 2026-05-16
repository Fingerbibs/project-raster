using UnityEngine;

public  class FpsMovement : MovementType
{
    private Vector3 lockedPosition;

    public override void HandleMovement()
    {
        // Lock position in place
        characterController.enabled = false;
        transform.position = lockedPosition;
        characterController.enabled = true;
    }

    public void SetLockPosition() => lockedPosition = transform.position;
}
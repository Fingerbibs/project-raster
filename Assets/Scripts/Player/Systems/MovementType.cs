using UnityEngine;

public abstract class MovementType : MonoBehaviour
{
    protected PlayerController playerController;
    protected CharacterController characterController;
    public float moveSpeed = 0f;

    protected virtual void Awake()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
    }

    public abstract void HandleMovement();

    public float GetMoveSpeed() => moveSpeed;
}
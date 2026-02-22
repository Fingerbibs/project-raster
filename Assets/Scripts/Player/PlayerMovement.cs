using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private MovementState currentState = MovementState.Free;

    private InputActions playerInput;
    private CharacterController characterController;
    private PlayerCoverSystem coverSystem;
    private Transform cameraTransform;

    private Vector2 movementInput;
    private bool isMovementPressed;

    #region SETUP

    private void Awake()
    {
        playerInput = new InputActions();
        characterController = GetComponent<CharacterController>();
        coverSystem = GetComponent<PlayerCoverSystem>();

        cameraTransform = Camera.main.transform;

        playerInput.Player.Move.performed += OnMovementInput;
        playerInput.Player.Move.canceled += OnMovementInput;
    }

    private void OnEnable() => playerInput.Player.Enable();
    private void OnDisable() => playerInput.Player.Disable();

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        isMovementPressed = movementInput.sqrMagnitude > 0.01f;
    }

    #endregion

    #region UPDATE

    private void Update()
    {
        switch (currentState)
        {
            case MovementState.Free:
                HandleFreeMovement();
                break;
            case MovementState.Cover:
                coverSystem.HandleCoverMovement();
                break;
            case MovementState.FirstPerson:
                //HandleFirstPersonMovement();
                break;
            
        }
    }

    #endregion

    #region MOVEMENT

    private void HandleFreeMovement()
    {
        Vector3 moveDirection = GetPlayerMovementDirection();

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            RotateTowards(moveDirection);
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void RotateTowards(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    #endregion

    #region HELPERS

    public void SetState(MovementState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        Debug.Log($"State changed: {currentState} → {newState}");
    }

    public MovementState GetState()
    {
        return currentState;
    } 

    public Vector3 GetPlayerMovementDirection()
    {
        if (movementInput.sqrMagnitude < 0.01f)
            return Vector3.zero;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        return camForward * movementInput.y +
               camRight * movementInput.x;
    }

    public bool IsRunning() => currentState == MovementState.Free && isMovementPressed;

    #endregion
}
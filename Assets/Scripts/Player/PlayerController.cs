using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private StateMachine<MovementState, PlayerContext> stateMachine;
    private InputActions playerInput;
    private CharacterController characterController;

    // MOVEMENT TYPES
    private CoverMovement coverMovement;
    private FreeMovement freeMovement;
    private FpsMovement fpsMovement;

    private Transform cameraTransform;
    private Vector2 movementInput;
    private MovementState previousState = MovementState.Free;

    private void Awake()
    {
        playerInput = new InputActions();
        characterController = GetComponent<CharacterController>();
        coverMovement = GetComponent<CoverMovement>();
        freeMovement = GetComponent<FreeMovement>();
        fpsMovement = GetComponent<FpsMovement>();

        cameraTransform = Camera.main.transform;

        playerInput.Player.Move.performed += OnMovementInput;
        playerInput.Player.Move.canceled += OnMovementInput;

        playerInput.Player.FirstPersonToggle.performed += _ => 
        {
            coverMovement.enabled = false;
            SetState(MovementState.FirstPerson);
        };
        playerInput.Player.FirstPersonToggle.canceled += _ => {
            coverMovement.enabled = true;
            SetState(previousState);
        };

        coverMovement.OnCoverEntered += () => SetState(MovementState.Cover);
        coverMovement.OnCoverExited  += () => SetState(MovementState.Free);

        InitStateMachine();
    }

    private void Update() => stateMachine.Update();

    #region State Machine
    private void InitStateMachine()
    {
        var context = new PlayerContext(
            this,
            characterController,
            transform,
            coverMovement,
            freeMovement,
            fpsMovement
        );


        var states = new Dictionary<MovementState, IState<MovementState>>
        {
            { MovementState.Free,        new FreeState(context) },
            { MovementState.Cover,       new CoverState(context) },
            { MovementState.FirstPerson, new FirstPersonState(context) },
        };

        stateMachine = new StateMachine<MovementState, PlayerContext>(states, MovementState.Free);
    }

    public void SetState(MovementState next)
    {
        previousState = stateMachine.CurrentState;
        stateMachine.TransitionTo(next);
    }

    public MovementState GetState() => stateMachine.CurrentState;
    #endregion

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public Vector3 GetMovementDirection()
    {
        if (movementInput.sqrMagnitude < 0.01f)
            return Vector3.zero;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight   = cameraTransform.right;

        camForward.y = 0f;
        camRight.y   = 0f;

        camForward.Normalize();
        camRight.Normalize();

        return camForward * movementInput.y + camRight * movementInput.x;
    }

    public Vector3 GetNormalizedMovement()
    {
        Vector3 moveDir = GetMovementDirection();
        return moveDir.magnitude < 0.1f ? Vector3.zero : moveDir.normalized;
    }
    
    public bool IsRunning() => GetState() == MovementState.Free && movementInput.sqrMagnitude > 0.01f;
    public bool IsInCover() => GetState() == MovementState.Cover;

    private void OnEnable()  => playerInput.Player.Enable();
    private void OnDisable() => playerInput.Player.Disable();
}
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerFirstPerson : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    private InputActions playerInput;
    private PlayerCoverSystem coverSystem;
    private PlayerMovement playerMovement;


    private bool isInFirstPerson = false;
    private Vector3 lockedPosition;
    

    private void Awake()
    {
        playerInput = new InputActions();
        controller = GetComponent<CharacterController>();
        coverSystem = GetComponent<PlayerCoverSystem>();
        playerMovement = GetComponent<PlayerMovement>();

        playerInput.Player.FirstPersonToggle.performed += ToggleFirstPerson;
        playerInput.Player.FirstPersonToggle.canceled += ToggleFirstPerson;
    }

    private void OnEnable() => playerInput.Player.Enable();
    private void OnDisable() => playerInput.Player.Disable();

    private void ToggleFirstPerson(InputAction.CallbackContext context)
    {
        isInFirstPerson = !isInFirstPerson;

        if (isInFirstPerson)
            EnterFirstPerson();
        else
            ExitFirstPerson();
    }

    private void EnterFirstPerson()
    {
        lockedPosition = transform.position;

        if (coverSystem != null)
            coverSystem.enabled = false;
        
        playerMovement.SetState(MovementState.FirstPerson);
    }

    private void ExitFirstPerson()
    {
        if (coverSystem != null)
            coverSystem.enabled = true;

        playerMovement.SetState(MovementState.Free);
    }

    private void LateUpdate()
    {
        if (isInFirstPerson)
        {
            // Lock the player in place
            controller.enabled = false;          // Temporarily disable to move manually
            transform.position = lockedPosition;
            controller.enabled = true;
        }
    }
}

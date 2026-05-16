using UnityEngine;

public class FreeState : IMovementState
{
    private readonly PlayerContext c;

    public FreeState(PlayerContext context) => c = context;

    public void Enter() { }

    public void Update() => c.FreeMove.HandleMovement();

    public void Exit() { }

    public bool CanTransitionTo(MovementState next) => true;
}
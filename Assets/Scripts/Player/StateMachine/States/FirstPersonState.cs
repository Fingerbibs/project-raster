using System.Security;
using UnityEngine;

public class FirstPersonState : IMovementState
{
    private readonly PlayerContext c;

    public FirstPersonState(PlayerContext context) => c = context;

    public void Enter()
    {
        c.FpsMove.SetLockPosition();
        if (c.CoverMove != null) c.CoverMove.enabled = false;
    }

    public void Update() => c.FpsMove.HandleMovement();

    public void Exit()
    {
        if (c.CoverMove != null) c.CoverMove.enabled = true;
    }

    public bool CanTransitionTo(MovementState next) => true;
}
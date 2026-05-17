using UnityEngine;
using System;
public class FirstPersonState : IState<MovementState>
{
    private readonly PlayerContext c;
    
    public FirstPersonState(PlayerContext context) => c = context;

    public void Enter()
    {
        c.FpsMove.SetLockPosition();
    }

    public void Update() => c.FpsMove.HandleMovement();

    public void Exit() { }

    public bool CanTransitionTo(MovementState next) => true;
}
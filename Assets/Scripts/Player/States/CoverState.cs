using UnityEngine;
public class CoverState : IState<MovementState>
{
    private readonly PlayerContext c;

    public CoverState(PlayerContext context) => c = context;

    public void Enter(){}

    public void Update() => c.CoverMove.HandleMovement();

    public void Exit() => c.CoverMove.ResetCover();

    public bool CanTransitionTo(MovementState next) => true;
}
public interface IMovementState
{
    void Enter();
    void Update();
    void Exit();
    bool CanTransitionTo(MovementState next);
}
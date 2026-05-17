public interface IState<TState>
{
    void Enter();
    void Update();
    void Exit();
    bool CanTransitionTo(TState next);
}
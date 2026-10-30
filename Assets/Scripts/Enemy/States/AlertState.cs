using UnityEngine;

public class AlertState : IState<EnemyState>
{
    private readonly EnemyContext c;

    public AlertState(EnemyContext context) => c = context;

    public void Enter() => Debug.Log($"{c.Transform.name} alerted");

    public void Update() { }

    public void Exit() { }

    public bool CanTransitionTo(EnemyState next) => true;
}

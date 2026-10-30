using UnityEngine;

public class AlertState : IState<EnemyState>
{
    private readonly EnemyContext c;

    public AlertState(EnemyContext context) => c = context;

    public void Enter() => c.EnemyChase.StartChase();

    public void Update() => c.EnemyChase.Chase(c.EnemyVision.LastKnownPlayerPosition, c.EnemyVision.CanSeePlayer());

    public void Exit() => c.EnemyChase.StopChase();

    public bool CanTransitionTo(EnemyState next) => true;
}

using UnityEngine;

public class EvasionState : IState<EnemyState>
{
    private readonly EnemyContext c;

    public EvasionState(EnemyContext context) => c = context;

    public void Enter() => c.EnemySearch.StartSearch(c.Enemy.SearchTarget);

    public void Update() => c.EnemySearch.Search();

    public void Exit() => c.EnemySearch.StopSearch();

    public bool CanTransitionTo(EnemyState next) => true;
}

using UnityEngine;
public class EnemyContext
{
    public readonly EnemyController Enemy;
    public readonly Transform Transform;

    public readonly EnemyPatrol EnemyPatrol;
    public readonly EnemyVision EnemyVision;
    public readonly EnemyChase EnemyChase;
    public readonly EnemySearch EnemySearch;

    public EnemyContext(EnemyController enemy, Transform transform, EnemyPatrol enemyPatrol, EnemyVision enemyVision,
        EnemyChase enemyChase, EnemySearch enemySearch)
    {
        Enemy     = enemy;
        Transform  = transform;

        EnemyPatrol = enemyPatrol;
        EnemyVision = enemyVision;
        EnemyChase  = enemyChase;
        EnemySearch = enemySearch;
    }
}
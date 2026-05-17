using UnityEngine;
public class EnemyContext
{
    public readonly EnemyController Enemy;
    public readonly Transform Transform;

    public EnemyContext(EnemyController enemy, Transform transform)
    {
        Enemy     = enemy;
        Transform  = transform;
    }
}
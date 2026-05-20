using UnityEngine;
public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private EnemyController enemyController;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyController = GetComponent<EnemyController>();
    }

    void Update()
    {
        animator.SetBool("isWalking", enemyController.IsWalking());
        animator.SetBool("isIdle", enemyController.IsIdle());
    }
}
using UnityEngine;
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        animator.SetBool("isRunning", playerController.IsRunning());
        animator.SetBool("isInCover", playerController.IsInCover());
    }
}
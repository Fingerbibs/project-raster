using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerCoverSystem coverSystem;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        coverSystem = GetComponent<PlayerCoverSystem>();
    }

    void Update()
    {
        animator.SetBool("isRunning", playerMovement.IsRunning());
        animator.SetBool("isInCover", coverSystem.IsInCover());
    }
}
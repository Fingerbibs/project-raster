using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Global Camera")]
    [SerializeField] private CinemachineCamera coverCamera;
    [SerializeField] private CinemachineCamera firstPersonCamera;

    private CinemachineCamera currentExploreCamera;

    private PlayerMovement playerMovement;

    private const int EXPLORE_PRIORITY = 10;
    private const int COVER_PRIORITY = 20;
    private const int FIRST_PERSON_PRIORITY = 30;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if(!playerMovement)
            Debug.LogError("CameraManager: No PlayerMovement found in scene!");
    }

    private void LateUpdate()
    {
        if(playerMovement == null) return;

        UpdatePriorities(playerMovement.currentState);
    }

    private void UpdatePriorities(MovementState state)
    {
        // Reset all
        if (currentExploreCamera != null) currentExploreCamera.Priority = 0;
        if (coverCamera != null) coverCamera.Priority = 0;
        if (firstPersonCamera != null) firstPersonCamera.Priority = 0;

        switch (state)
        {
            case MovementState.FirstPerson:
                if(firstPersonCamera != null) firstPersonCamera.Priority = FIRST_PERSON_PRIORITY;
                break;
            case MovementState.Cover:
                if (coverCamera != null) coverCamera.Priority = COVER_PRIORITY;
                break;
            case MovementState.Free:
                if (currentExploreCamera != null) currentExploreCamera.Priority = EXPLORE_PRIORITY;
                break;
        }
    }

     // Room camera assignment
    public void SetExploreCamera(CinemachineCamera exploreCam)
    {
        currentExploreCamera = exploreCam;
    }
}

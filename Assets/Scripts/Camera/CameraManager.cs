using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Global Camera")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCoverSystem coverSystem;

    [Header("Cameras")]
    [SerializeField] private CinemachineCamera coverCamera;
    [SerializeField] private CinemachineCamera firstPersonCamera;
    private CinemachineCamera currentExploreCamera;

    private CinemachineThirdPersonFollow coverCameraFollow;
    private CinemachinePanTilt fpPanTilt;

    private const int INACTIVE_PRIORITY = 0;
    private const int EXPLORE_PRIORITY = 10;
    private const int COVER_PRIORITY = 20;
    private const int FIRST_PERSON_PRIORITY = 30;

    private CinemachineCamera activeCamera;

    private void Awake()
    {
        if(!playerMovement)
            Debug.LogError("CameraManager: No PlayerMovement found in scene!");
        

        fpPanTilt = firstPersonCamera.GetComponent<CinemachinePanTilt>();
        coverCameraFollow = coverCamera.GetComponent<CinemachineThirdPersonFollow>();
    }

    private void LateUpdate()
    {
        if(playerMovement == null) return;

        UpdatePriorities(playerMovement.GetState());
        UpdateCoverCameraSide();
    }

    private void UpdatePriorities(MovementState state)
    {
        // Reset all
        if (currentExploreCamera != null) currentExploreCamera.Priority = INACTIVE_PRIORITY;
        if (coverCamera != null) coverCamera.Priority = INACTIVE_PRIORITY;
        if (firstPersonCamera != null) firstPersonCamera.Priority = INACTIVE_PRIORITY;

        switch (state)
        {
            case MovementState.FirstPerson:
                if(firstPersonCamera != null) firstPersonCamera.Priority = FIRST_PERSON_PRIORITY;

                if (activeCamera != firstPersonCamera)
                {
                        fpPanTilt.PanAxis.Value = 0;
                        fpPanTilt.TiltAxis.Value = 0;
                }
                activeCamera = firstPersonCamera;

                break;

            case MovementState.Cover:
                // Only switch to cover camera when near an edge
                if (coverSystem.IsNearEdge())
                {
                    if (coverCamera != null) coverCamera.Priority = COVER_PRIORITY;
                }
                else
                {
                    if (currentExploreCamera != null) currentExploreCamera.Priority = EXPLORE_PRIORITY;
                }
                break;
                
            case MovementState.Free:
                if (currentExploreCamera != null) currentExploreCamera.Priority = EXPLORE_PRIORITY;
                activeCamera = currentExploreCamera;

                break;
        }
    }

    private void UpdateCoverCameraSide()
    {
        if (coverCameraFollow == null || coverSystem == null) return;
        if (!coverSystem.IsInCover()) return;

        bool rightOpen = coverSystem.IsRightSideOpen();
        bool leftOpen = coverSystem.IsLeftSideOpen();

        if (rightOpen && leftOpen)
            coverCameraFollow.CameraSide = coverSystem.GetLastMoveDirection() > 0f ? 0f : 1f;
        else if (rightOpen)
            coverCameraFollow.CameraSide = 0f;
        else if (leftOpen)
            coverCameraFollow.CameraSide = 1f;
    }

     // Room camera assignment
    public void SetExploreCamera(CinemachineCamera exploreCam)
    {
        currentExploreCamera = exploreCam;
    }

    public CinemachineCamera GetActiveCamera() => activeCamera;
}
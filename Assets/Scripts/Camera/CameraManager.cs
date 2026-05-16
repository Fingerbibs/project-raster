using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [Header("Cameras")]
    [SerializeField] private CoverCamera coverCamera;
    [SerializeField] private FirstPersonCamera firstPersonCamera;
    [SerializeField] private ExploreCamera exploreCamera;

    private CinemachineCamera activeCamera;

    private const int INACTIVE_PRIORITY = 0;
    private const int EXPLORE_PRIORITY = 10;
    private const int COVER_PRIORITY = 20;
    private const int FIRST_PERSON_PRIORITY = 30;

    private void Awake()
    {
        if(!playerController)
            Debug.LogError("CameraManager: No PlayerController found in scene!");
    }

    private void LateUpdate()
    {
        if(playerController == null) return;
        UpdatePriorities(playerController.GetState());
    }

    private void UpdatePriorities(MovementState state)
    {
        // Reset all priorities
        exploreCamera.SetPriority(INACTIVE_PRIORITY);
        coverCamera.SetPriority(INACTIVE_PRIORITY);
        firstPersonCamera.SetPriority(INACTIVE_PRIORITY);

        switch (state)
        {
            case MovementState.FirstPerson:
                firstPersonCamera.SetPriority(FIRST_PERSON_PRIORITY);

                if (activeCamera != firstPersonCamera.GetCamera())
                        firstPersonCamera.SetInitView();
                
                activeCamera = firstPersonCamera.GetCamera();
                break;
            case MovementState.Cover:
                // Only switch to cover camera when near an edge
                if (coverCamera.IsNearEdge())
                {
                    coverCamera.SetPriority(COVER_PRIORITY);
                    activeCamera = coverCamera.GetCamera();
                }
                else
                {
                    exploreCamera.SetPriority(EXPLORE_PRIORITY);
                    activeCamera = exploreCamera.GetCamera();
                }
                break;
            case MovementState.Free:
                exploreCamera.SetPriority(EXPLORE_PRIORITY);
                activeCamera = exploreCamera.GetCamera();
                break;
        }
    }
    
    public CinemachineCamera GetActiveCamera() => activeCamera;
}
using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Cameras")]
    [SerializeField] private CoverCamera coverCamera;
    [SerializeField] private FirstPersonCamera firstPersonCamera;
    [SerializeField] private ExploreCamera exploreCamera;

    private BaseCamera activeCamera;

    private const int INACTIVE_PRIORITY = 0;
    private const int EXPLORE_PRIORITY = 10;
    private const int COVER_PRIORITY = 20;
    private const int FIRST_PERSON_PRIORITY = 30;

    private void Awake()
    {
        if(!playerMovement)
            Debug.LogError("CameraManager: No PlayerMovement found in scene!");
    }

    private void LateUpdate()
    {
        if(playerMovement == null) return;
        UpdatePriorities(playerMovement.GetState());
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

                if (activeCamera != firstPersonCamera)
                        firstPersonCamera.SetInitView();
                
                activeCamera = firstPersonCamera;
                break;
            case MovementState.Cover:
                // Only switch to cover camera when near an edge
                if (coverCamera.IsNearEdge())
                {
                    coverCamera.SetPriority(COVER_PRIORITY);
                    activeCamera = coverCamera;
                }
                else
                {
                    exploreCamera.SetPriority(EXPLORE_PRIORITY);
                    activeCamera = exploreCamera;
                }
                break;
            case MovementState.Free:
                exploreCamera.SetPriority(EXPLORE_PRIORITY);
                activeCamera = exploreCamera;
                break;
        }
    }
    
    public BaseCamera GetActiveCamera() => activeCamera;
}
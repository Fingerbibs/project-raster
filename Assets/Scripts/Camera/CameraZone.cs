using UnityEngine;
using Unity.Cinemachine;

public class CameraZone : MonoBehaviour
{
    [SerializeField] private CinemachineCamera exploreCamera;
    [SerializeField] private CameraManager cameraManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraManager.SetExploreCamera(exploreCamera);
        }
    }
}

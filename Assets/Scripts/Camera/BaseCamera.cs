public class BaseCamera : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;

    protected void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    public CinemachineCamera GetCamera() => cinemachineCamera;

    public void SetCamera(CinemachineCamera newCamera)
    {
        cinemachineCamera = newCamera;
    }

    public void SetPriority(int priority)
    {
        if (cinemachineCamera != null) 
            cinemachineCamera.Priority = priority;
    }
}

using UnityEngine;

public class CameraZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ZoneEvents.onCameraZoneEnter?.Invoke(this);
    }
}

using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CoverCamera : BaseCamera
{
    [SerializeField] private PlayerCoverSystem coverSystem;

    private CinemachineThirdPersonFollow cameraFollow;
    private Coroutine cameraSideCoroutine;

    private void Awake()
    {
        base.Awake();
        cameraFollow = GetComponent<CinemachineThirdPersonFollow>();
    }

    private void Update()
    {
        UpdateCameraSide();
    }

    public bool IsNearEdge()
    {
        return coverSystem.IsNearEdge();
    }

    private void UpdateCameraSide()
    {
        if (cameraFollow == null || coverSystem == null) return;
        if (!coverSystem.IsInCover()) return;
        if (cameraSideCoroutine != null) return; // don't interrupt ongoing transition

        cameraSideCoroutine = StartCoroutine(UpdateCameraSideCoroutine());
    }

    private IEnumerator UpdateCameraSideCoroutine()
    {
        bool rightOpen = coverSystem.IsRightSideOpen();
        bool leftOpen = coverSystem.IsLeftSideOpen();

        float targetSide;
        if (rightOpen && leftOpen)
            targetSide = coverSystem.GetLastMoveDirection() > 0f ? 0f : 1f;
        else if (rightOpen)
            targetSide = 0f;
        else if (leftOpen)
            targetSide = 1f;
        else
            yield break;

        float startSide = cameraFollow.CameraSide;
        float elapsed = 0f;
        float duration = 0.2f; // tweak this

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cameraFollow.CameraSide = Mathf.Lerp(startSide, targetSide, elapsed / duration);
            yield return null;
        }

        cameraFollow.CameraSide = targetSide;
        cameraSideCoroutine = null; // allow next transition to trigger
    }
}

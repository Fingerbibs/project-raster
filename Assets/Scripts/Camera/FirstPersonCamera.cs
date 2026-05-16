using UnityEngine;
using Unity.Cinemachine;

public class FirstPersonCamera : BaseCamera
{
    [SerializeField] private ReticleController reticle;
    private CinemachinePanTilt panTilt;

    private const int FIRST_PERSON_PRIORITY = 30;

    private void Awake()
    {
        base.Awake();
        panTilt = GetComponent<CinemachinePanTilt>();
    }

    private void Update()
    {
        if (GetCamera().Priority == FIRST_PERSON_PRIORITY)
        {
            var brain = CinemachineCore.FindPotentialTargetBrain(GetCamera());
            bool blending = brain != null && brain.IsBlending;
            reticle.SetVisible(true);
        }
        else
            reticle.SetVisible(false);
    }

    public void SetInitView()
    {
        panTilt.PanAxis.Value = 0;
        panTilt.TiltAxis.Value = 0;
    }
}
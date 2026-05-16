using UnityEngine;
using UnityEngine.UI;
public class ReticleController : MonoBehaviour
{
    [SerializeField] private Image _reticleImage;

    public void Awake()
    {
        _reticleImage.enabled = false;
    }

    public void SetVisible(bool visible)
    {
        if (visible)
            _reticleImage.enabled = true;
        else
            _reticleImage.enabled = false;
    }
}
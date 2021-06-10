using UnityEngine;

public enum CameraMode
{
    WOW,
    Fixed,
    Static,
    Doom,
    Platformer
}

public class CameraController : MonoBehaviour
{
    Camera m_MainCamera;
    ICamera m_ActiveCamera;
    public GameObject m_TargetGo;

    // TODO make get/set

    void Start()
    {
        this.m_MainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        this.FollowTarget();
    }

    public void SetTarget(GameObject _go)
    {
        m_TargetGo = _go;

        if (this.m_MainCamera == null)
            this.m_MainCamera = Camera.main;
    }

    public void SetMode(CameraMode _cameraMode)
    {
        switch (_cameraMode)
        {
            case CameraMode.Static:
                m_ActiveCamera = new PacManCamera();
                break;
            case CameraMode.Doom:
                m_ActiveCamera = new DoomCamera();
                break;
            case CameraMode.Fixed:
                m_ActiveCamera = new SpaceCraftCamera();
                break;
            case CameraMode.WOW:
                m_ActiveCamera = new WowCamera();
                break;
            case CameraMode.Platformer:
                m_ActiveCamera = new PlatformerCamera();
                break;
        }
    }

    void FollowTarget()
    {
        if (this.m_TargetGo == null)
            return;

        m_ActiveCamera.FollowTarget(this.m_TargetGo, this.m_MainCamera);
    }
}
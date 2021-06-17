using UnityEngine;

namespace Forbes.Cameras
{
    public class CameraController : MonoBehaviour
    {
        Camera m_MainCamera;
        ICamera m_ActiveCamera;
        public GameObject m_TargetGo;

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
                    m_ActiveCamera = new StaticCamera();
                    break;
                case CameraMode.FirstPerson:
                    m_ActiveCamera = new FirstPersonCamera();
                    break;
                case CameraMode.Fixed:
                    m_ActiveCamera = new FixedCamera();
                    break;
                case CameraMode.Wow:
                    m_ActiveCamera = new WowCamera();
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
}
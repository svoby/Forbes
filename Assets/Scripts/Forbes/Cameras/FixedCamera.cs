using UnityEngine;

namespace Forbes.Cameras
{
    public class FixedCamera : ICamera
    {
        Vector3 m_CameraInitPosition;
        public float CameraSmooth = 0;
        private Camera _camera;

        public FixedCamera()
        {
            m_CameraInitPosition = Camera.main.transform.position;
        }

        public void FollowTarget(GameObject _tar, Camera _camera)
        {
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, m_CameraInitPosition + _tar.transform.position, 1 / CameraSmooth);
        }
    }
}
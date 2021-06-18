using UnityEngine;
using Forbes.SinglePlayer;

namespace Forbes.Cameras
{
    public class FirstPersonCamera : ICamera
    {
        public float HeadBobbingMax = 0.03f;
        public float HeadBobbingSpeed = 10f;
        public float m_HeadBobbingAmplitude = 0.01f;
        public float HeadBobbingDump = 0.2f;
        float m_HeadBobbingTime = 0;
        Vector3 Offset = new Vector3(0, 0.48f, 0.12f);

        public FirstPersonCamera()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void FollowTarget(GameObject _tar, Camera _camera)
        {
            _camera.transform.parent = GameManager.Instance.LocalPlayer.MC.Aiming.transform;
            _camera.transform.localPosition = Offset;

            // Head bobbing
            m_HeadBobbingTime += HeadBobbingSpeed * Time.fixedDeltaTime;

            Vector3 _v = GameManager.Instance.LocalPlayer.MC.PhysicX.V;
            _v.y = 0;
            _v = Vector3.ClampMagnitude(_v, 1f);

            if (_v.magnitude > 0)
            {
                m_HeadBobbingAmplitude += _v.magnitude / 2 * Time.fixedDeltaTime;
                m_HeadBobbingAmplitude = m_HeadBobbingAmplitude > HeadBobbingMax ? HeadBobbingMax : m_HeadBobbingAmplitude;
            }

            m_HeadBobbingAmplitude -= HeadBobbingDump * Time.fixedDeltaTime;
            m_HeadBobbingAmplitude = (m_HeadBobbingAmplitude < 0) ? 0 : m_HeadBobbingAmplitude;

            _camera.transform.localPosition += new Vector3((m_HeadBobbingAmplitude) * Mathf.Cos(m_HeadBobbingTime / 2), m_HeadBobbingAmplitude * Mathf.Sin(m_HeadBobbingTime), 0);
        }
    }
}
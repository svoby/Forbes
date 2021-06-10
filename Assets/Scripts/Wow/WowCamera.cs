using UnityEngine;

internal class WowCamera : ICamera
{
    Vector3 m_Offset= new Vector3(0, 0, 14f);
    private bool m_CameraLocked;
    private float m_LerpTo;
    private float cameraSmooth;

    public WowCamera()
    {
        cameraSmooth = 3f;
    }

    public void FollowTarget(GameObject _tar, Camera _camera)
    {
        _camera.transform.position = new Vector3(_tar.transform.position.x, _tar.transform.position.y + 0.5f, _tar.transform.position.z);

        InputState _state = GameManager.Instance.InputController.State;

        // Camera free look?
        if (_state.RightClick)
        {
            m_Offset.x = m_Offset.x - _state.MouseY * GameManager.Instance.InputController.MouseLookSensitivity.y;
            m_Offset.y = m_Offset.y + _state.MouseX * GameManager.Instance.InputController.MouseLookSensitivity.x;
            m_CameraLocked = true;
        }

        // Lock cursor
        if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2"))
        {
            m_CameraLocked = false;
        }

        // Player and camera rotate
        if (!m_CameraLocked && _state.PlayerRotate != 0)
            m_Offset.y = m_Offset.y + _state.PlayerRotate * GameManager.Instance.InputController.KeyboardLookSensitivity * Time.fixedDeltaTime;

        // Rotate camera
        _camera.transform.rotation = Quaternion.Euler(m_Offset.x, m_Offset.y, _camera.transform.rotation.eulerAngles.z);

        // Zoom camera
        if (_state.MouseScrollDelta != 0)
        {
            m_Offset.z -= _state.MouseScrollDelta;
            m_Offset.z = Mathf.Clamp(m_Offset.z, 0.1f, 10f);
        }

        // Check camera collide
        RaycastHit _hit;
        Vector3 _cameraDirection = _camera.transform.TransformDirection(Vector3.back);
        Physics.Raycast(_tar.transform.position, _cameraDirection, out _hit, m_Offset.z, 1 << LayerMask.NameToLayer("Floor"));

        if (_hit.collider != null)
        {
            m_LerpTo = _hit.distance - 0.5f;
        }
        else
        {
            if (m_LerpTo < m_Offset.z)
            {
                m_LerpTo += cameraSmooth * ((m_Offset.z - m_LerpTo) / 1f) * Time.fixedDeltaTime;
            }

            if (m_LerpTo > m_Offset.z)
            {
                m_LerpTo -= cameraSmooth * ((m_LerpTo - m_Offset.z) / 1f) * Time.fixedDeltaTime;
            }
        }

        // Final lerp
        _camera.transform.Translate(Vector3.back * m_LerpTo);
    }
}
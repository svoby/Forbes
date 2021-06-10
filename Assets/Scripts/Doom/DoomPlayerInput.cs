using UnityEngine;

public class DoomPlayerInput : IInputType
{
    float m_RotationAngleY;
    float m_RotationSpeed = 160;
    float m_MouseSensitivity = 600;

    Vector3 _dir;

    public InputFrame GetInputFrame(GameObject _go)
    {
        InputController _input = GameManager.Instance.InputController;

        if (_input.Horizontal < 0) {
            m_RotationAngleY -= m_RotationSpeed * Time.fixedDeltaTime;
        }

        if (_input.Horizontal > 0) {
            m_RotationAngleY += m_RotationSpeed * Time.fixedDeltaTime;
        }

        m_RotationAngleY += _input.MouseX * m_MouseSensitivity * Time.deltaTime;
        _dir = Quaternion.Euler(0, m_RotationAngleY, 0) * new Vector3(_input.Strafe, 0, _input.Vertical);

        if (_input.Strafe != 0) {
            
        }

        InputFrame _frame = new InputFrame
        {
            Horizontal = _dir.x,
            Vertical = _dir.z,
            Jump = _input.Jump,
            Rotation = m_RotationAngleY,
            Fire = _input.Fire,
            Mask = InputMask.ForceBodyRotate,
            Timestamp = Time.time
        };

        return _frame;
    }
}

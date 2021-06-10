using UnityEngine;

public class PlatformerPlayerInput : IInputType
{
    float _newAngleY;

    public InputFrame GetInputFrame(GameObject _go)
    {
        InputController _input = GameManager.Instance.InputController;
        _newAngleY = Camera.main.transform.rotation.eulerAngles.y;

        Vector3 _inputDir = Quaternion.Euler(0, _newAngleY, 0) * new Vector3(_input.Horizontal, 0, _input.Vertical);

        InputFrame _frame = new InputFrame
        {
            Horizontal = _inputDir.x,
            Vertical = 0,
            Jump = _input.Jump,
            Rotation = 180,
            Fire = 0,
            Timestamp = Time.time
        };

        return _frame;
    }
}

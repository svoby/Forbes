using UnityEngine;

public class PacManPlayerInput : IInputType
{
    public InputFrame GetInputFrame(GameObject _go)
    {
        InputController _input = GameManager.Instance.InputController;
        InputFrame _frame = new InputFrame
        {
            Horizontal = _input.Horizontal,
            Vertical = _input.Vertical,
            Jump = false,
            Rotation = 0,
            Fire = 0,
            Timestamp = Time.time
        };

        return _frame;
    }
}

using UnityEngine;
using Forbes.Inputs;
using Forbes.SinglePlayer;

namespace Forbes.Tests
{
    public class TestInput2D : MonoBehaviour, IInputType
    {
        public InputFrame GetInputFrame(GameObject _go)
        {
            InputController _input = GameManager.Instance.InputController;
            InputFrame _frame = new InputFrame
            {
                Horizontal = _input.Horizontal,
                Vertical = _input.Vertical,
                Jump = _input.Jump,
                Rotation = 0,
                Fire = 0,
                Timestamp = Time.time
            };

            return _frame;
        }
    }
}
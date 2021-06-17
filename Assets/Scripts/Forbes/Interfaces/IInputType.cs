using UnityEngine;

namespace Forbes.Inputs
{
    public interface IInputType
    {
        Forbes.Inputs.InputFrame GetInputFrame(GameObject _go);
    }
}
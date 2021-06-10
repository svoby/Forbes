using UnityEngine;

public class SpaceCraftPlayerInput : IInputType
{
    float _newAngleY;

    public InputFrame GetInputFrame(GameObject _go)
    {
        InputController _input = GameManager.Instance.InputController;
        _newAngleY = Camera.main.transform.rotation.eulerAngles.y;
        float rotationAngleY = 0;

        // Rotation - Mouse 2.5D
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0;

        Ray camRay = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, 300f, LayerMask.GetMask("Floor")))
        {
            Vector3 cursorDirection3D = floorHit.point - _go.transform.position;
            Vector2 pointIn2DPlane = new Vector2(cursorDirection3D.x, cursorDirection3D.z);
            rotationAngleY = -Vector2.Angle(pointIn2DPlane, new Vector2(0, 1));
            rotationAngleY *= cursorDirection3D.x > 0 ? -1 : 1;
        }

        Vector3 _inputDir = Quaternion.Euler(0, _newAngleY, 0) * new Vector3(_input.Horizontal, 0, _input.Vertical);

        InputFrame _frame = new InputFrame
        {
            Horizontal = _inputDir.x,
            Vertical = _inputDir.z,
            Jump = _input.Jump,
            Rotation = rotationAngleY,
            Fire = _input.Fire,
            Timestamp = Time.time
        };

        return _frame;
    }
}

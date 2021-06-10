using UnityEngine;

public class WowPlayerInput : IInputType
{
    Vector2 m_lastCursorDirection2D;
    bool m_mouseMoved;
    private float newAngleY;

    public InputFrame GetInputFrame(GameObject _go)
    {
        InputController inputController = GameManager.Instance.InputController;
        float rotationAngleY = 0;

        // Rotation - Mouse 2.5D
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0;

        Ray camRay = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit floorHit;
        Vector2 newCursorDirecition2D = new Vector2(0, 0);

        if (Physics.Raycast(camRay, out floorHit, 300f, LayerMask.GetMask("Floor")))
        {
            newCursorDirecition2D = new Vector2(floorHit.point.x, floorHit.point.z);
            if (m_lastCursorDirection2D != newCursorDirecition2D)
            {
                Vector3 cursorDirection3D = floorHit.point - _go.transform.position;
                Vector2 pointIn2DPlane = new Vector2(cursorDirection3D.x, cursorDirection3D.z);
                rotationAngleY = -Vector2.Angle(pointIn2DPlane, new Vector2(0, 1));
                rotationAngleY *= cursorDirection3D.x > 0 ? -1 : 1;
                m_mouseMoved = true;
            }
        }

        // Rotate by mouse or joystick
        if (m_mouseMoved)
        {
            m_lastCursorDirection2D = newCursorDirecition2D;
        }
        else
        {
            // Rotation - JoyStick
        }

        // Move by camera rotation
        newAngleY = Camera.main.transform.rotation.eulerAngles.y;
        Vector3 inputDir = Quaternion.Euler(0, newAngleY, 0) * new Vector3(inputController.Horizontal, 0, inputController.Vertical);

        InputFrame input = new InputFrame
        {
            Horizontal = inputDir.x,
            Vertical = inputDir.z,
            Jump = inputController.Jump,
            Rotation = rotationAngleY,
            Fire = inputController.Fire,
            Timestamp = Time.time
        };

        return input;
    }
}
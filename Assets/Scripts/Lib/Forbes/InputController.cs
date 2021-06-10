using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerTypes {
    Wow,
    SpaceCraft,
    PacMan,
    Doom,
    Platformer
}

public class InputController : MonoBehaviour
{
    InputState m_State;
    public InputState State
    {
        get
        {
            if (m_State == null)
                m_State = new InputState();
            return m_State;
        }
        set
        {
            m_State = value;
        }
    }

    // Input types
    List<IInputType> m_InutTypes;
    IInputType m_ActiveType;

    public float Horizontal { get { return State.Horizontal; } }
    public float Vertical { get { return State.Vertical; } }
    public float Rotation { get { return State.Rotation; } }
    public bool Jump { get { return State.Jump; } }
    public int Fire { get { return State.Fire; } }
    public float MouseX { get { return State.MouseX; } }
    public float Strafe { get { return State.Strafe; } }
    public bool RecordingToggler { get { return State.RecordingToggler; } }
    public bool LoadReplay { get { return State.LoadReplay; } set { State.LoadReplay = value; } }
    public bool Key1 { get { return State.Key1; } }
    public bool Key2 { get { return State.Key2; } }
    public bool Key3 { get { return State.Key3; } }

    Vector2 m_lastCursorDirection2D;
    Vector2 m_lastJoystickDirection2D;

    bool m_mouseMoved;
    float m_timeScale = 1f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public float MouseLookSensitivityX = 20f;
	public float MouseLookSensitivityY = 5f;
	public Vector2 MouseLookSensitivity = new Vector2(20f, 10f);
    public float KeyboardLookSensitivity = 150f;
    private float newAngleY;

    void Awake()
    {
        // Set input types
        m_InutTypes = new List<IInputType>
        {
            new WowPlayerInput(),
            new SpaceCraftPlayerInput(),
            new PacManPlayerInput(),
            new DoomPlayerInput(),
            new PlatformerPlayerInput(),
        };
    }

    void FixedUpdate()
    {
        State = new InputState();

        // Movement
        State.Horizontal = Input.GetAxisRaw("Horizontal");
        State.Vertical = Input.GetAxisRaw("Vertical");

        // Wow type strafe
        if (Input.GetKey(KeyCode.Q))
            State.Strafe = -1;
        
        if (Input.GetKey(KeyCode.E))
            State.Strafe = 1;

        // Jump request
        if (Input.GetButton("Jump"))
            State.Jump = GameManager.Instance.LocalPlayer.MC.PhysicX.IsGrounded;

        // Fire
        State.Fire = 0;
        if (Input.GetButton("Fire1"))
            State.Fire = 1;

        // Mouse
        State.MouseX = Input.GetAxis("Mouse X");
        State.MouseY = Input.GetAxis("Mouse Y");
        State.LeftClick = Input.GetButton("Fire1");
        State.LeftClickUp = Input.GetButtonUp("Fire1");
        State.LeftClickDown = Input.GetButtonDown("Fire1");
        State.RightClick = Input.GetButton("Fire2");
        State.RightClickUp = Input.GetButtonUp("Fire2");
        State.RightClickDown = Input.GetButtonDown("Fire2");
		State.MouseScrollDelta = Input.mouseScrollDelta.y;

        State.Key1 = Input.inputString == "+";
        State.Key2 = Input.inputString == "ě";
        State.Key3 = Input.inputString == "š";

        if(Input.GetKey(KeyCode.AltGr) && Input.GetKey(KeyCode.Alpha2))
            Debug.Log("KeyCode down: AltGr & 2 equals ě");
    }

    public void SetTypeID(int _id)
    {
        m_ActiveType = m_InutTypes[_id];
    }

    public InputFrame GetInputFrame(GameObject _go)
    {
        return m_ActiveType.GetInputFrame(_go);
    }
}
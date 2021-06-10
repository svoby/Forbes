using System;
using UnityEngine;

public enum GameLogics
{
    WowGameLogic,
    SpaceCraft,
    PacMan
}

[RequireComponent(typeof(MonsterController))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    // Crosshair
    #region Variables

    [Header("Crosshair")]
    public GameObject Crosshair;
    public float CrosshairMaxDistance = 10f;
    public float CrosshairLerp = 0.3f;
    float _crosshairLerpTo = 5f;

    [Header("Camera")]
    public CameraMode CameraMode;

    [Header("InputController")]
    public ControllerTypes ControllerType;

    [Header("UI")]
    [SerializeField] GameObject UIDamageFlash;

    #endregion

    // Monster controller
    MonsterController m_MC;
    public MonsterController MC
    {
        get
        {
            if (m_MC == null)
                m_MC = gameObject.GetComponent<MonsterController>();
            return m_MC;
        }
    }

    InputFrame InputFrame;

    void Awake()
    {
        // Set camera
        GameManager.Instance.OnLocalPlayerJoined += (IPlayerController _pc) => GameManager.Instance.CameraController.SetTarget(this.gameObject);
        GameManager.Instance.OnLocalPlayerJoined += (IPlayerController _pc) => GameManager.Instance.CameraController.SetMode(CameraMode);

        // Set controller type
        GameManager.Instance.OnLocalPlayerJoined += (IPlayerController _pc) => GameManager.Instance.InputController.SetTypeID((int)ControllerType);
        GameManager.Instance.OnLocalPlayerJoined += (IPlayerController _pc) => GameManager.Instance.GameLogic = gameObject.GetComponent<IGameLogic>();

        // Set damage flash
        GameManager.Instance.OnLocalPlayerJoined += (IPlayerController _pc) => gameObject.GetComponent<Destructable>().OnDamageReceived += () =>
        {
            if (UIDamageFlash == null) return;
            UIDamageFlash.SetActive(true);
            GameManager.Instance.Timer.Add(() => UIDamageFlash.SetActive(false), 0.3f);
        };
    }

    void Start()
    {
        GameManager.Instance.LocalPlayer = this;
    }

    void FixedUpdate()
    {
        InputFrame = GameManager.Instance.InputController.GetInputFrame(gameObject);
        MC.ApplyInputs(InputFrame);
    }

    // void ShowCrosshair()
    // {
    //     if (Crosshair == null)
    //         return;

    //     //int layerMask = 1 << 9;
    //     RaycastHit hit;
    //     Vector3 spawnDirection = MC.BulletSpawn.TransformDirection(Vector3.forward);

    //     Physics.Raycast(MC.BulletSpawn.position, spawnDirection * CrosshairMaxDistance, out hit, CrosshairMaxDistance, ~(1 << LayerMask.NameToLayer("Pickup") | 1 << LayerMask.NameToLayer("Walkable") | 1 << LayerMask.NameToLayer("Damage")));

    //     Vector3 bckPos = Crosshair.transform.position;
    //     Vector3 bckLocalPos = new Vector3(Crosshair.transform.localPosition.x, Crosshair.transform.localPosition.y, Crosshair.transform.localPosition.z);
    //     float bckLocalPosZ = Crosshair.transform.localPosition.z; // actual z backup

    //     Crosshair.transform.position = MC.BulletSpawn.position; // Reset position

    //     if (hit.collider != null)
    //     {
    //         _crosshairLerpTo = hit.distance - 0.2f;
    //     }
    //     else
    //     {
    //         if (_crosshairLerpTo < CrosshairMaxDistance)
    //         {
    //             _crosshairLerpTo += CrosshairLerp * (bckLocalPosZ / CrosshairMaxDistance);
    //         }
    //     }

    //     Crosshair.transform.Translate(Vector3.forward * _crosshairLerpTo);
    // }
}

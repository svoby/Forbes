using UnityEngine;
using Forbes.Inputs;
using Forbes.Cameras;

namespace Forbes.SinglePlayer
{
    [RequireComponent(typeof(MonsterController))]
    [RequireComponent(typeof(IInputType))]
    [RequireComponent(typeof(IGameLogic))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        // Crosshair
        #region Variables

        [Header("Camera")]
        public CameraMode CameraMode;

        [Header("UI")]
        [SerializeField] GameObject UIDamageFlash;

        #endregion

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
            GameManager.Instance.OnLocalPlayerJoined += (IPlayerController _pc) =>
            {
                // Set camera
                GameManager.Instance.CameraController.SetTarget(this.gameObject);
                GameManager.Instance.CameraController.SetMode(CameraMode);

                // Set controller type
                GameManager.Instance.InputController.SetType = _pc.MC.GetComponent<IInputType>();
                GameManager.Instance.GameLogic = _pc.MC.GetComponent<IGameLogic>();

                // Set damage flash
                Destructable des = _pc.MC.GetComponent<Destructable>();
                if (des != null)
                    des.OnDamageReceived += () =>
                    {
                        if (UIDamageFlash == null) return;
                        UIDamageFlash.SetActive(true);
                        GameManager.Instance.Timer.Add(() => UIDamageFlash.SetActive(false), 0.3f);
                    };
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
    }
}
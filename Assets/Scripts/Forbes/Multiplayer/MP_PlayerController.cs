using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using Forbes.Inputs;
using Forbes.Cameras;

namespace Forbes
{
    [RequireComponent(typeof(MonsterController))]
    [RequireComponent(typeof(NetworkObject))]
    public class MP_PlayerController : NetworkBehaviour, IPlayerController
    {
        // Crosshair
        #region Variables

        [Header("Camera")]
        public CameraMode CameraMode;

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
            GameManager.Instance.OnLocalPlayerJoined += (IPlayerController _pc) => GameManager.Instance.CameraController.SetTarget(_pc.MC.gameObject);
            GameManager.Instance.OnLocalPlayerJoined += (IPlayerController _pc) => GameManager.Instance.CameraController.SetMode(CameraMode);
            GameManager.Instance.OnLocalPlayerJoined += (IPlayerController _pc) => GameManager.Instance.InputController.SetType = _pc.MC.GetComponent<IInputType>();
            GameManager.Instance.OnLocalPlayerJoined += (IPlayerController _pc) => GameManager.Instance.GameLogic = _pc.MC.GetComponent<IGameLogic>();
        }

        void Start()
        {
            if (!IsLocalPlayer)
                return;

            GameManager.Instance.LocalPlayer = this;
        }

        void FixedUpdate()
        {
            if (IsLocalPlayer)
            {
                InputFrame = GameManager.Instance.InputController.GetInputFrame(gameObject);
                MC.ApplyInputs(InputFrame);

                if (!IsServer)
                    SubmitInputRequestServerRpc(new MP_InputFrame
                    {
                        Horizontal = InputFrame.Horizontal,
                        Vertical = InputFrame.Vertical,
                        Jump = InputFrame.Jump,
                        Fire = InputFrame.Fire,
                        Rotation = InputFrame.Rotation
                    });
            }
        }

        [ServerRpc]
        void SubmitInputRequestServerRpc(MP_InputFrame _inputFrame)
        {
            MC.ApplyInputs(new InputFrame
            {
                Horizontal = _inputFrame.Horizontal,
                Vertical = _inputFrame.Vertical,
                Jump = _inputFrame.Jump,
                Fire = _inputFrame.Fire,
                Rotation = _inputFrame.Rotation
            });
        }
    }
}
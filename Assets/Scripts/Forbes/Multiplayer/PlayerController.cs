using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using Forbes.Inputs;
using Forbes.Cameras;

namespace Forbes.Multiplayer
{
    [RequireComponent(typeof(MonsterController))]
    [RequireComponent(typeof(NetworkObject))]
    public class PlayerController : NetworkBehaviour, IPlayerController
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

        Forbes.Inputs.InputFrame InputFrame;

        void Awake()
        {
            Forbes.SinglePlayer.GameManager.Instance.OnLocalPlayerJoined += (IPlayerController _pc) =>
            {
                Forbes.SinglePlayer.GameManager.Instance.CameraController.SetTarget(_pc.MC.gameObject);
                Forbes.SinglePlayer.GameManager.Instance.CameraController.SetMode(CameraMode);
                Forbes.SinglePlayer.GameManager.Instance.InputController.SetType = _pc.MC.GetComponent<IInputType>();
                Forbes.SinglePlayer.GameManager.Instance.GameLogic = _pc.MC.GetComponent<IGameLogic>();
            };
        }

        void Start()
        {
            if (!IsLocalPlayer)
                return;

            Forbes.SinglePlayer.GameManager.Instance.LocalPlayer = this;
        }

        void FixedUpdate()
        {
            if (IsLocalPlayer)
            {
                InputFrame = Forbes.SinglePlayer.GameManager.Instance.InputController.GetInputFrame(gameObject);
                MC.ApplyInputs(InputFrame);

                if (!IsServer)
                    SubmitInputRequestServerRpc(new InputFrame
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
        void SubmitInputRequestServerRpc(InputFrame _inputFrame)
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
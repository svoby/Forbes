using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using Forbes.Inputs;
using Forbes.Cameras;

namespace Forbes.Multiplayer
{
    [RequireComponent(typeof(MonsterController))]
    [RequireComponent(typeof(NetworkObject))]
    [RequireComponent(typeof(IInputType))]
    public class PlayerController : NetworkBehaviour, IPlayerController
    {
        // Crosshair
        #region Variables

        [Header("Camera")]
        public CameraMode CameraMode;

        #endregion

        Vector3 m_LastPosition;

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
            m_LastPosition = transform.position;
        }

        void FixedUpdate()
        {
            if (IsLocalPlayer)
            {
                InputFrame = Forbes.SinglePlayer.GameManager.Instance.InputController.GetInputFrame(gameObject);
                MC.ApplyInputs(InputFrame);

                if (IsServer && Vector3.Distance(m_LastPosition, transform.position) > 0.5f)
                {
                    PacketToClientRpc(new PacketClientTransfrom
                    {
                        Position = transform.position,
                        V = MC.PhysicX.V
                    });

                    m_LastPosition = transform.position;
                }

                if (!IsServer)
                    PacketToServerRpc(new InputFrame
                    {
                        Horizontal = InputFrame.Horizontal,
                        Vertical = InputFrame.Vertical,
                        Jump = false,
                        Fire = InputFrame.Fire,
                        Rotation = InputFrame.Rotation
                    });
            }
        }

        [ServerRpc]
        void PacketToServerRpc(InputFrame _inputFrame)
        {
            MC.ApplyInputs(new InputFrame
            {
                Horizontal = _inputFrame.Horizontal,
                Vertical = _inputFrame.Vertical,
                Jump = _inputFrame.Jump,
                Fire = _inputFrame.Fire,
                Rotation = _inputFrame.Rotation
            });

            PacketToClientRpc(new PacketClientTransfrom
            {
                Position = transform.position,
                V = MC.PhysicX.V
            });

            // new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new[] { NetworkManager.Singleton.ConnectedClientsList[i].ClientId } } });
        }

        [ClientRpc]
        void PacketToClientRpc(PacketClientTransfrom packet)
        {
            transform.position = packet.Position;
            MC.PhysicX.V = packet.V;
        }
    }
}
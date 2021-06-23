using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using Forbes.Inputs;
using Forbes.Cameras;
using System;

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

        // Caches
        InputFrame m_InputToServer = new InputFrame();
        InputFrame m_InputToClient = new InputFrame();
        InputFrame m_InputReceived = new InputFrame();
        PacketToServer m_PacketToServer = new PacketToServer();
        PacketToClient m_PacketToClient = new PacketToClient();

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
            m_LastPosition = transform.position;

            if (IsLocalPlayer)
                Forbes.SinglePlayer.GameManager.Instance.LocalPlayer = this;
        }

        void FixedUpdate()
        {
            if (IsServer)
                this.CorrectPosition(0.5f);

            if (IsLocalPlayer)
            {
                m_InputToServer = Forbes.SinglePlayer.GameManager.Instance.InputController.GetInputFrame(gameObject);
                MC.ApplyInputs(m_InputToServer);

                if (!IsServer) // IsClient & Owner?
                {
                    m_PacketToServer.Horizontal = m_InputToServer.Horizontal;
                    m_PacketToServer.Vertical = m_InputToServer.Vertical;
                    m_PacketToServer.Jump = m_InputToServer.Jump;
                    m_PacketToServer.Fire = m_InputToServer.Fire;
                    m_PacketToServer.Rotation = m_InputToServer.Rotation;

                    PacketToServerRpc(m_PacketToServer);
                };
            }
        }

        private void CorrectPosition(float treshold)
        {
            if (Vector3.Distance(m_LastPosition, transform.position) > treshold)
            {
                m_LastPosition = transform.position;

                m_PacketToClient.Position = transform.position;
                m_PacketToClient.V = MC.PhysicX.V;

                PacketToClientRpc(m_PacketToClient);
            }
        }

        [ServerRpc]
        void PacketToServerRpc(PacketToServer input)
        {
            m_InputReceived.Horizontal = input.Horizontal;
            m_InputReceived.Vertical = input.Vertical;
            m_InputReceived.Jump = input.Jump;
            m_InputReceived.Horizontal = input.Horizontal;
            m_InputReceived.Fire = input.Fire;
            m_InputReceived.Rotation = input.Rotation;

            MC.ApplyInputs(m_InputReceived);

            // new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new[] { NetworkManager.Singleton.ConnectedClientsList[i].ClientId } } });
        }

        [ClientRpc]
        void PacketToClientRpc(PacketToClient packet)
        {
            transform.position = packet.Position;
            MC.PhysicX.V = packet.V;
        }
    }
}
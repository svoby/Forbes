using UnityEngine;
using MLAPI;
using MLAPI.Extensions;

namespace Forbes.Multiplayer
{
    public class GameManager : MonoBehaviour
    {
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId,
                    out var networkedClient))
                {
                    // var player = networkedClient.PlayerObject.GetComponent<HelloWorldPlayer>();
                    // if (player)
                    // {
                    //     player.Move();
                    // }
                }
            }
        }

        private static Spawner m_Spawner;
        public static Spawner Spawner
        {
            get
            {
                if (m_Spawner == null)
                    m_Spawner = new Spawner();

                return m_Spawner;
            }
        }

        private static NetworkObjectPool m_ObjectPool;
        public static NetworkObjectPool ObjectPool
        {
            get
            {
                if (m_ObjectPool == null)
                    m_ObjectPool = GameObject.FindWithTag("ObjectPool").GetComponent<NetworkObjectPool>();

                return m_ObjectPool;
            }
        }
    }
}
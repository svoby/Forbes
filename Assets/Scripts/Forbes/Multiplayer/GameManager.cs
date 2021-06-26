using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Extensions;

namespace Forbes.Multiplayer
{
    public class GameManager : MonoBehaviour
    {
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

        void Start()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.OnServerStarted += () => 
            {
               ulong? prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("PlayerMP");
                GameManager.Spawner.Spawn(prefabHash, Vector3.zero, Quaternion.identity, NetworkManager.Singleton.LocalClientId);
            };
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkManager.ConnectionApprovedDelegate callback)
        {
            bool approve = true;
            bool createPlayerObject = true;
            ulong? prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("PlayerMP");

            callback(createPlayerObject, prefabHash, approve, Vector3.zero, Quaternion.identity);
        }

        void OnGUI()
        {
            GUI.color = Color.white;
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
            GUI.color = Color.black;
            GUILayout.Label("Mode: " + (NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client"));
            GUILayout.Label("LocalClientId: " + NetworkManager.Singleton.LocalClientId);
        }
    }
}
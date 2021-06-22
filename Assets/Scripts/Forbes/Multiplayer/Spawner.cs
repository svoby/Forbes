using UnityEngine;
using MLAPI;

namespace Forbes.Multiplayer
{
    public class Spawner : NetworkBehaviour, ISpawner
    {
        public void Spawn(ulong prefabHash, Vector3 pos, Quaternion rot)
        {
            // Get prefab from pool
            var go = GameManager.ObjectPool.GetNetworkObject(prefabHash, pos, rot);

            // Sub spawns
            foreach (var c in go.GetComponents<ISpawn>())
                c.Spawn();

            // Network spawn
            go.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
        }

        public void Despawn(GameObject go)
        {
            foreach (var c in go.GetComponents<IDespawn>())
                c.Despawn();

            go.GetComponent<NetworkObject>().Despawn();
            GameManager.ObjectPool.ReturnNetworkObject(go.GetComponent<NetworkObject>());
        }

        void LogSpawn()
        {
            ulong id = NetworkManager.Singleton.LocalClientId;
            NetworkObject po = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
            Debug.Log("PlyerObject name: " + po.name);
            Debug.Log("PlyerObject IsLocalPlayer: " + po.IsLocalPlayer);
            Debug.Log("Spawn id:" + id);
        }
    }
}
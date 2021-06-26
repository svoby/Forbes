using UnityEngine;
using MLAPI;

namespace Forbes.Multiplayer
{
    public class Spawner : NetworkBehaviour, ISpawner
    {
        public void Spawn(ulong? prefabHash, Vector3 pos, Quaternion rot, ulong? clientId)
        {
            // Get prefab from pool
            var go = GameManager.ObjectPool.GetNetworkObject((ulong) prefabHash, pos, rot);
            var nwo = go.GetComponent<NetworkObject>();

            // Sub spawns
            foreach (var c in go.GetComponents<ISpawn>())
                c.Spawn();

            // Is nwo player object?
            if (clientId != null)
            {
                nwo.SpawnAsPlayerObject((ulong)clientId);
            }
            else
            {
                nwo.Spawn();
            }
        }

        public void Despawn(GameObject go)
        {
            var nwo = go.GetComponent<NetworkObject>();

            foreach (var c in go.GetComponents<IDespawn>())
                c.Despawn();

            nwo.Despawn();
            GameManager.ObjectPool.ReturnNetworkObject(nwo);
        }
    }
}
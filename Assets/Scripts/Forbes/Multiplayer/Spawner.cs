using UnityEngine;
using MLAPI;

namespace Forbes.Multiplayer
{
    public class Spawner : NetworkBehaviour, ISpawner
    {
        public GameObject Spawn(GameObject go, Vector3 pos, Quaternion rot)
        {
            GameObject clone = go;

            if (!go.scene.isLoaded)
                clone = Instantiate(go, pos, rot);

            foreach (var c in clone.GetComponents<ISpawn>())
                c.Spawn();

            clone.SetActive(true);

            if (IsServer)
                clone.GetComponent<NetworkObject>().Spawn();

            return clone;
        }

        public void Despawn(GameObject go)
        {
            Forbes.SinglePlayer.GameManager.Spawner.Despawn(go);

            foreach (var c in go.GetComponents<IDespawn>())
                c.Despawn();
        }
    }
}


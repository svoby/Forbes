using UnityEngine;
using MLAPI;
//using MLAPI.Exte

namespace Forbes.Multiplayer
{
  //  static string s_ObjectPoolTag = "ObjectPool";
   // NetworkObjectPool m_ObjectPool;

    public class Spawner : NetworkBehaviour, ISpawner
    {
        void LogSpawn()
        {
            ulong id = NetworkManager.Singleton.LocalClientId;
            NetworkObject po = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
            Debug.Log("PlyerObject name: " + po.name);
            Debug.Log("PlyerObject IsLocalPlayer: " + po.IsLocalPlayer);
            Debug.Log("Spawn id:" + id);
        }

        public GameObject Spawn(GameObject go, Vector3 pos, Quaternion rot)
        {
            GameObject clone = go;

            if (!go.scene.isLoaded)
                clone = Instantiate(go, pos, rot);

            clone.SetActive(true);

            foreach (var c in clone.GetComponents<ISpawn>())
                c.Spawn();

            if (IsServer)
                clone.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
                //var go = m_ObjectPool.GetNetworkObject(NetworkObject.PrefabHash, transform.position + diff, Quaternion.identity);

            return clone;
        }

        public void Despawn(GameObject go)
        {
            foreach (var c in go.GetComponents<IDespawn>())
                c.Despawn();

            go.SetActive(false);
            go.GetComponent<NetworkObject>().Despawn();
            //Destroy(go);
        }
    }
}


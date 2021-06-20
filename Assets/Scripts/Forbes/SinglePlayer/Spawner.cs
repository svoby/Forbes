using UnityEngine;

namespace Forbes.SinglePlayer
{
    public class Spawner : MonoBehaviour, ISpawner
    {
        public GameObject Spawn(GameObject go, Vector3 pos, Quaternion rot)
        {
            GameObject clone = go;

            if (!go.scene.isLoaded)
                clone = Instantiate(go, pos, rot);

            foreach (var c in clone.GetComponents<ISpawn>())
                c.Spawn();

            go.SetActive(true);
            return clone;
        }

        public void Despawn(GameObject go)
        {
            foreach (var c in go.GetComponents<IDespawn>())
                c.Despawn();

            go.SetActive(false);
        }
    }
}
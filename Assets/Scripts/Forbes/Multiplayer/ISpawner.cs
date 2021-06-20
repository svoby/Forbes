using UnityEngine;

namespace Forbes.Multiplayer
{
    public interface ISpawner
    {
        GameObject Spawn(GameObject go, Vector3 pos, Quaternion rot);
        void Despawn(GameObject go);
    }
}

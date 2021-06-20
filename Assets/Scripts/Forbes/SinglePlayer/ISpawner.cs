using UnityEngine;

namespace Forbes.SinglePlayer
{
    public interface ISpawner
    {
        GameObject Spawn(GameObject go, Vector3 pos, Quaternion rot);
        void Despawn(GameObject go);
    }
}
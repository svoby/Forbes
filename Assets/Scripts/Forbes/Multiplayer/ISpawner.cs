using UnityEngine;

namespace Forbes.Multiplayer
{
    public interface ISpawner
    {
        void Spawn(ulong playerHash, Vector3 pos, Quaternion rot);
        void Despawn(GameObject go);
    }
}

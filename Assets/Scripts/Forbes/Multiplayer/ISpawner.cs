using UnityEngine;

namespace Forbes.Multiplayer
{
    public interface ISpawner
    {
        void Spawn(ulong? playerHash, Vector3 pos, Quaternion rot, ulong? clientId);
        void Despawn(GameObject go);
    }
}
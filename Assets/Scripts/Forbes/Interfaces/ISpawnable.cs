using UnityEngine;

namespace Forbes.Spawning
{
    public interface ISpawn
    {
        event System.Action<GameObject> OnSpawn;
        event System.Action<GameObject> OnDespawn;

        void Spawn();
        void Despawn();
    }
}
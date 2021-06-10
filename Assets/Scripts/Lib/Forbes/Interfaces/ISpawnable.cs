using UnityEngine;

public interface ISpawn
{
    event System.Action<GameObject> OnSpawn;
    event System.Action<GameObject> OnDespawn;

    void Spawn();
    void Despawn();
}
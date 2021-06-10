using System;
using UnityEngine;

public class RespawnSelf : MonoBehaviour, ISpawn
{
    public event Action<GameObject> OnSpawn;
    public event Action<GameObject> OnDespawn;

    public float RespawnTime;
    Vector3 m_InitPosition;
    Quaternion m_InitRotation;
    public AudioClip SoundRespawn;

    void Awake()
    {
        m_InitPosition = transform.position;
        m_InitRotation = transform.rotation;

        OnSpawn += (GameObject go) =>
        {
            transform.position = m_InitPosition;
            transform.rotation = m_InitRotation;

            if (SoundRespawn != null)
                AudioSource.PlayClipAtPoint(SoundRespawn, transform.position);
        };

        OnDespawn += (GameObject go) =>
        {
            if (RespawnTime > 0)
                GameManager.Instance.Timer.Add(() => GameManager.Instance.Spawner.Spawn(this), RespawnTime);
        };
    }

    public void Spawn()
    {
        if (OnSpawn != null)
            OnSpawn(gameObject);
    }

    public void Despawn()
    {
        if (OnDespawn != null)
            OnDespawn(gameObject);
    }
}
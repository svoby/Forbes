using UnityEngine;
using MLAPI;

namespace Forbes.Multiplayer
{
    public class RespawnSelf : NetworkBehaviour, IDespawn
    {
        [Header("Multiplayer Respawn time")]
        public float RespawnTime;

        Vector3 m_InitPosition;
        Quaternion m_InitRotation;

        void Awake()
        {
            m_InitPosition = transform.position;
            m_InitRotation = transform.rotation;
        }

        public void Despawn()
        {
            if (RespawnTime > 0)
                Forbes.SinglePlayer.GameManager.Instance.Timer.Add(() => GameManager.Spawner.Spawn(gameObject, m_InitPosition, m_InitRotation), RespawnTime);
        }
    }
}
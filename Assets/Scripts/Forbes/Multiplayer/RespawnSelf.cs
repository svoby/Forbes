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

        void OnDisable()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        public void Despawn()
        {
            if (RespawnTime > 0)
                Forbes.SinglePlayer.GameManager.Instance.Timer.Add(() =>
                    GameManager.Spawner.Spawn(NetworkObject.PrefabHash, Vector3.zero, Quaternion.identity), RespawnTime);
        }
    }
}
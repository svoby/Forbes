using UnityEngine;
using MLAPI;

namespace Forbes.Multiplayer
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(NetworkObject))]
    public class Pickup : NetworkBehaviour, IPickup
    {
        public bool IsPickedUp { get; set; }

        private void OnEnable()
        {
            IsPickedUp = false;
        }

        private void OnDisable()
        {
            IsPickedUp = true;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (!IsServer || IsPickedUp)
                return;

            GameManager.Spawner.Despawn(gameObject);

            collider.gameObject.GetComponent<Destructable>()?.Heal(50);
        }
    }
}
using UnityEngine;

using MLAPI;

namespace Forbes.SinglePlayer
{
    [RequireComponent(typeof(SphereCollider))]
    public class Pickup : MonoBehaviour, IPickup
    {
        public event System.Action<GameObject> OnPickedUp;

        [Header("Sounds")]
        public AudioClip SoundPickup;

        public bool IsPickedUp { get; set; }

        void Pick(GameObject who)
        {
            if (SoundPickup != null)
                AudioSource.PlayClipAtPoint(SoundPickup, transform.position);

            GameManager.Spawner.Despawn(gameObject);
        }

        private void OnEnable()
        {
            OnPickedUp += Pick;
            IsPickedUp = false;
        }

        private void OnDisable()
        {
            OnPickedUp -= Pick;
            IsPickedUp = true;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (IsPickedUp) return;

            if (OnPickedUp != null)
                OnPickedUp(collider.gameObject);
        }
    }
}
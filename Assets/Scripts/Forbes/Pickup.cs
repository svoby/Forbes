using UnityEngine;
using Forbes.SinglePlayer;

namespace Forbes
{
    [RequireComponent(typeof(SphereCollider))]
    public class Pickup : MonoBehaviour, IPickup
    {
        public event System.Action<GameObject> OnPickedUp;

        [Header("Sounds")]
        public AudioClip SoundPickup;

        public bool IsPickedUp { get; set; }

        void OnTriggerEnter(Collider collider)
        {
            if (IsPickedUp)
                return;

            if (SoundPickup != null)
                AudioSource.PlayClipAtPoint(SoundPickup, transform.position);

            if (OnPickedUp != null)
                OnPickedUp(collider.gameObject);

            IsPickedUp = true;

            GameManager.Instance.Spawner.Despawn(gameObject);
        }
    }
}
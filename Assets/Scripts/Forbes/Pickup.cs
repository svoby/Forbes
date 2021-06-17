using UnityEngine;

namespace Forbes
{
    [RequireComponent(typeof(SphereCollider))]
    public class Pickup : MonoBehaviour, IPickup
    {
        public event System.Action<GameObject> OnPickedUp;

        [Header("Item Props")]
        [SerializeField]
        private string itemName;
        [SerializeField]
        private int itemCount;
        [SerializeField]
        private int itemMax;

        [Header("Sounds")]
        public AudioClip SoundPickup;

        public bool IsPickedUp { get; set; }

        public string ItemName { get => itemName; set => itemName = value; }
        public int ItemCount { get => itemCount; set => itemCount = value; }
        public int ItemMax { get => itemMax; set => itemMax = value; }

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
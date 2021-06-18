using UnityEngine;
using Forbes.SinglePlayer;

namespace Forbes
{
    public class Destructable : MonoBehaviour
    {
        [SerializeField] int hitPoints;
        public event System.Action OnDeath;
        public event System.Action OnDamageReceived;

        [Header("Sounds")]
        public AudioClip SoundDie;
        public AudioClip SoundPain;

        [Header("DamageText")]
        public GameObject DamageTextPrefab;

        int damageTaken;
        float nextPainTime;
        [SerializeField] float nextPainTimeout = 1.5f;

        Vector3 m_InitPosition;

        [SerializeField]

        private void OnEnable()
        {
            damageTaken = 0;
        }

        void Start()
        {
            m_InitPosition = transform.position;
        }

        public float HitPointsRemaining
        {
            get
            {
                return hitPoints - damageTaken;
            }
        }

        public bool IsAlive
        {
            get
            {
                return HitPointsRemaining > 0;
            }
        }

        public virtual void TakeDamage(int amount, GameObject _damageOwner)
        {
            if (!IsAlive)
                return;

            damageTaken += amount;

            if (Time.time > nextPainTime)
            {
                nextPainTime = Time.time + nextPainTimeout;

                if (SoundPain != null && HitPointsRemaining > 0)
                    AudioSource.PlayClipAtPoint(SoundPain, transform.position);
            }

            if (OnDamageReceived != null)
                OnDamageReceived();

            // Target killed
            if (HitPointsRemaining <= 0)
            {
                this.Die();
            }
        }

        public virtual void Heal(int amount)
        {
            damageTaken -= amount;
        }

        public virtual void Die()
        {
            if (SoundDie != null)
                AudioSource.PlayClipAtPoint(SoundDie, transform.position);

            damageTaken = hitPoints;

            GameManager.Instance.Spawner.Despawn(this.gameObject);

            if (OnDeath != null)
                OnDeath();
        }
    }
}
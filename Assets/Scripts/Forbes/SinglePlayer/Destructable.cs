using UnityEngine;

namespace Forbes.SinglePlayer
{
    public class Destructable : MonoBehaviour, Forbes.Damage.IDestructable
    {
        [SerializeField] int m_HitPoints = 100;
        public event System.Action OnDeath;
        public event System.Action OnDamageReceived;

        [Header("Sounds")]
        public AudioClip SoundDie;
        public AudioClip SoundPain;

        [Header("DamageText")]
        public GameObject DamageTextPrefab;

        int m_DamageTaken;
        float m_NextPainTime;
        float m_NextPainTimeout = 1.5f;

        private void OnEnable()
        {
            m_DamageTaken = 0;
        }

        public float HitPointsRemaining
        {
            get
            {
                return m_HitPoints - m_DamageTaken;
            }
        }

        public bool IsAlive
        {
            get
            {
                return HitPointsRemaining > 0;
            }
        }

        public virtual void TakeDamage(int amount)
        {
            if (!IsAlive)
                return;

            m_DamageTaken += amount;

            this.PlayPain();

            if (OnDamageReceived != null)
                OnDamageReceived();

            if (HitPointsRemaining <= 0)
            {
                this.Die();
            }
        }

        private void PlayPain()
        {
            if (Time.time > m_NextPainTime)
            {
                m_NextPainTime = Time.time + m_NextPainTimeout;

                if (SoundPain != null && HitPointsRemaining > 0)
                    AudioSource.PlayClipAtPoint(SoundPain, transform.position);
            }
        }

        public virtual void Heal(int amount)
        {
            m_DamageTaken -= amount;
        }

        public virtual void Die()
        {
            if (SoundDie != null)
                AudioSource.PlayClipAtPoint(SoundDie, transform.position);

            m_DamageTaken = m_HitPoints;

            GameManager.Spawner.Despawn(this.gameObject);

            if (OnDeath != null)
                OnDeath();
        }

    }
}
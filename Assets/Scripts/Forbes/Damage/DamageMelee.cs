using UnityEngine;

namespace Forbes.Damage
{
    public class DamageMelee : MonoBehaviour, IProjectile
    {
        public int Damage = 10;
        public float DamageRadius = 2;
        public int DamageMultipler = 1;
        public float PushForce = 2f;

        [Header("Sounds")]
        public AudioClip SoundFire;

        private GameObject m_damageOwner;
        public GameObject DamageOwner { get { return m_damageOwner; } set { m_damageOwner = value; } }
        public GameObject Target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        void Start()
        {
            if (SoundFire != null)
                AudioSource.PlayClipAtPoint(SoundFire, transform.position);

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, DamageRadius, LayerMask.GetMask("Player", "Enemy"), QueryTriggerInteraction.UseGlobal);
            foreach (Collider collider in hitColliders)
            {
                var PhysicXTarget = collider.GetComponent<PhysicX>();
                if (PhysicXTarget != null)
                    PhysicXTarget.AddForce(transform.forward * PushForce);

                var destructable = collider.transform.GetComponent<Forbes.SinglePlayer.Destructable>();
                if (destructable != null)
                {
                    destructable.TakeDamage(Damage);
                }
            }

            DestroyProjectile();
        }

        void DestroyProjectile()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, DamageRadius);
        }
    }
}
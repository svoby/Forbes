using UnityEngine;
namespace Forbes.Damage
{
    class DamageRay : MonoBehaviour, IProjectile
    {
        private RaycastHit _hitInfo;

        public GameObject DamageOwner { get; set; }
        public GameObject Target { get; set; }
        public float PushForce;
        public int Damage;

        [Header("Prefabs")]
        public GameObject ExplosionPrefab;
        public GameObject SmokePrefab;
        public Transform ProjectileHolePrefab;

        void Start()
        {
            if (Physics.Raycast(transform.position, transform.forward, out _hitInfo, Mathf.Infinity, LayerMask.GetMask("Floor", "Player", "Enemy")))
            {
                var PhysicXTarget = _hitInfo.collider.GetComponent<PhysicX>();
                if (PhysicXTarget != null)
                    PhysicXTarget.AddForce(transform.forward * PushForce);

                var destructable = _hitInfo.collider.transform.GetComponent<Forbes.SinglePlayer.Destructable>();
                if (destructable != null)
                {
                    destructable.TakeDamage(Damage);
                }
                else
                {
                    this.MakeProjectileHole(_hitInfo);
                }

                this.MakeExplosion(_hitInfo.point);
            }

            Destroy(this);
        }

        void MakeExplosion(Vector3 _pos)
        {
            if (ExplosionPrefab == null)
                return;

            GameObject expl = Instantiate(ExplosionPrefab, _pos, transform.rotation);
            Destroy(expl, 4f);
        }

        public void MakeProjectileHole(RaycastHit _hitInfo)
        {
            if (ProjectileHolePrefab == null)
                return;

            Vector3 destination = _hitInfo.point + _hitInfo.normal * 0.005f;
            Transform hole = Instantiate(ProjectileHolePrefab, destination, Quaternion.LookRotation(_hitInfo.normal) * Quaternion.Euler(0, 180f, 0));
            Destroy(hole.transform.gameObject, 5f);
        }
    }
}
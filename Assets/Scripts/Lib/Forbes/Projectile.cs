using UnityEngine;

[RequireComponent(typeof(PhysicX))]
public class Projectile : MonoBehaviour, IProjectile
{
    [Header("Prefabs")]
    public GameObject ExplosionPrefab;

    public GameObject ProjectileHolePrefab;

    public int Damage = 10;
    public int Speed = 10;
    public int DamageMultipler = 1;
    public float PushForce = 2f;
    public int AutoDestroyTime = 10;
    public float SeekAccuracy = 1.5f;

    private RaycastHit _hitInfo;

    private GameObject m_damageOwner;
    public GameObject DamageOwner { get { return m_damageOwner; } set { m_damageOwner = value; } }

    private GameObject m_target = null;
    public GameObject Target { get { return m_target; } set { m_target = value; } }

    private PhysicX PhysicX;
    private float m_FixDelayTime = 0;

    void Start()
    {
        PhysicX = gameObject.GetComponent<PhysicX>();
        PhysicX.AddForce(transform.forward * Speed);

        if (AutoDestroyTime > 0)
            Invoke("DestroyProjectile", AutoDestroyTime);
    }

    void FixedUpdate()
    {
        if (Target != null)
        {
            Vector3 targetDir = Target.transform.position - transform.position;
            PhysicX.V = Vector3.Lerp(PhysicX.V, targetDir.normalized * Speed, SeekAccuracy * Time.fixedDeltaTime);
            transform.forward = PhysicX.V;
        }

        // Raycast for collision
        if (Physics.Raycast(transform.position, transform.forward, out _hitInfo, PhysicX.V.magnitude * Time.fixedDeltaTime, LayerMask.GetMask("Floor", "Player", "Enemy")))
        {
            _hitInfo.collider.GetComponent<PhysicX>()?.AddForce(transform.forward * PushForce);

            var destructable = _hitInfo.collider.GetComponent<Destructable>();
            if (destructable != null)
            {
                destructable.TakeDamage(Damage, DamageOwner);
            }
            else
            {
                MakeProjectileHole(_hitInfo);
            }

            transform.position = _hitInfo.point;
            DestroyProjectile();
        }

        if (PhysicX.IsGrounded)
            DestroyProjectile();
    }

    void MakeProjectileHole(RaycastHit _hitInfo)
    {
        if (ProjectileHolePrefab != null)
        {
            Vector3 destination = _hitInfo.point + _hitInfo.normal * 0.005f;
            Quaternion rotation = Quaternion.LookRotation(_hitInfo.normal) * Quaternion.Euler(0, 180f, 0);

            GameObject hole = GameManager.Instance.Spawner.Spawn(ProjectileHolePrefab, destination, rotation);
            GameManager.Instance.Timer.Add(() =>
                GameManager.Instance.Spawner.Despawn(hole, true), 3f);
        }
    }

    void DestroyProjectile()
    {
        if (ExplosionPrefab != null)
        {
            GameObject expl = GameManager.Instance.Spawner.Spawn(ExplosionPrefab, transform.position, transform.rotation);
            GameManager.Instance.Timer.Add(() =>
                GameManager.Instance.Spawner.Despawn(expl, true), 3f);
        }

        GameManager.Instance.Spawner.Despawn(gameObject, true);
    }
}
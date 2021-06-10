using System;
using UnityEngine;

public class DoomWeaponShotgun : MonoBehaviour, IWeapon
{
    public event Action<IWeapon> OnWeaponBeforeFired;
    public event Action<IWeapon> OnWeaponFired;

    [Header("WeaponConfig")]
    [SerializeField]
    private GameObject m_ProjectilePrefab;
    public GameObject ProjectilePrefab { get => m_ProjectilePrefab; set => m_ProjectilePrefab = value; }

    [SerializeField]
    private Transform m_BulletSpawn;
    public Transform BulletSpawn { get => m_BulletSpawn; set => m_BulletSpawn = value; }

    private GameObject m_DamageOwner;
    public GameObject DamageOwner { get => m_DamageOwner; set => m_DamageOwner = value; }

    public bool OutOfAmmo { get; set; }

    public float FireRate = 1f;
    private float m_NextFireTime;
    public float SpreadAmount;
    private float SpreadSuppresion = 0.5f;

    [Header("Sounds & Effects")]
    public AudioClip SoundFire;

    [SerializeField]
    ParticleSystem m_MuzzleFireParticle;
    public ParticleSystem MuzzleFireParticle { get => m_MuzzleFireParticle; }

    public int BulletAmount = 3;

    public bool CanFire()
    {
        if (OutOfAmmo)
            return false;

        if (Time.time > m_NextFireTime)
        {
            m_NextFireTime = Time.time + FireRate;
            return true;
        }

        return false;
    }

    public void Fire()
    {
        if (ProjectilePrefab == null) return;
        if (!CanFire()) return;

        if (OnWeaponBeforeFired != null)
            OnWeaponBeforeFired(this);

        for (int i = 0; i <= BulletAmount - 1; i++)
        {
            bool isEnemyInView = false;
            Quaternion rotation = BulletSpawn.transform.rotation;

            RaycastHit[] inView = Physics.BoxCastAll(BulletSpawn.position, new Vector3(1f, 20f, 0.01f), BulletSpawn.transform.forward, BulletSpawn.transform.rotation, 200f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore);
            foreach (RaycastHit enemy in inView)
            {
                Vector3 dir = enemy.collider.transform.position - BulletSpawn.position;

                RaycastHit _hitInfo;
                if (Physics.Raycast(BulletSpawn.position, dir, out _hitInfo, 200f, LayerMask.GetMask("Floor", "Enemy")))
                {
                    var destructable = _hitInfo.collider.transform.GetComponent<Destructable>();
                    if (destructable != null) {
                        rotation = Quaternion.LookRotation(dir, Vector3.up);
                        isEnemyInView = true;
                    }
                }
            }

            if (SpreadAmount != 0)
            {
                float randX = UnityEngine.Random.Range(-SpreadAmount, SpreadAmount);
                float randY = UnityEngine.Random.Range(-SpreadAmount, SpreadAmount);

                if (isEnemyInView) {
                    randX *= SpreadSuppresion;
                    randY *= SpreadSuppresion;
                }

                rotation = Quaternion.Euler(rotation.eulerAngles.x + randX, rotation.eulerAngles.y + randY, rotation.eulerAngles.z);
            }

            GameObject projectile = Instantiate(ProjectilePrefab, BulletSpawn.position, rotation);
            projectile.GetComponent<IProjectile>().DamageOwner = DamageOwner;
        }

        // Muzzle & sound
        if (MuzzleFireParticle != null)
            MuzzleFireParticle.Play();

        if (SoundFire != null)
            AudioSource.PlayClipAtPoint(SoundFire, BulletSpawn.transform.position);

        if (OnWeaponFired != null)
            OnWeaponFired(this);
    }

    public void Upgrade()
    {
        BulletAmount = 6;
    }
}

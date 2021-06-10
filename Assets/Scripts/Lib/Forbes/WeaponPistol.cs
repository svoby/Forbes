using System;
using UnityEngine;

public class WeaponPistol : MonoBehaviour, IWeapon
{
    [SerializeField]
    private GameObject m_ProjectilePrefab;
    public GameObject ProjectilePrefab { get => m_ProjectilePrefab; set => m_ProjectilePrefab = value; }

    [SerializeField]
    private Transform m_BulletSpawn;
    public Transform BulletSpawn { get => m_BulletSpawn; set => m_BulletSpawn = value; }

    private GameObject m_DamageOwner;

    public event Action<IWeapon> OnWeaponFired;
    public event Action<IWeapon> OnWeaponBeforeFired;

    public GameObject DamageOwner { get => m_DamageOwner; set => m_DamageOwner = value; }
    public bool OutOfAmmo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public WeaponPistol(GameObject _go, Transform _BulletSpawn)
    {
        m_DamageOwner = _go;
        m_BulletSpawn = _BulletSpawn;
    }

    public void Fire()
    {
        if (OnWeaponBeforeFired != null)
            OnWeaponBeforeFired(this);

        GameObject projectileClone = Instantiate(ProjectilePrefab, BulletSpawn.transform.position, BulletSpawn.transform.rotation);
        projectileClone.GetComponent<IProjectile>().DamageOwner = DamageOwner;
    }

    public bool CanFire()
    {
        throw new NotImplementedException();
    }

    public void Upgrade()
    {
        throw new NotImplementedException();
    }
}
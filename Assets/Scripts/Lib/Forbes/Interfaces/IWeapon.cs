using UnityEngine;

enum WeaponType
{
    pistol,
    shotgun,
    chaingun,
    rocket
}

public interface IWeapon
{
    event System.Action<IWeapon> OnWeaponBeforeFired;
    event System.Action<IWeapon> OnWeaponFired;

    GameObject ProjectilePrefab { get; set; }
    GameObject DamageOwner { get; set; }
    Transform BulletSpawn { get; set; }
    bool OutOfAmmo { get; set; }

    void Fire();
    bool CanFire();
    void Upgrade();
}
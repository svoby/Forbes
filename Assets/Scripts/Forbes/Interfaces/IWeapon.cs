using UnityEngine;

namespace Forbes
{
    public interface IWeapon
    {
        event System.Action<IWeapon> OnWeaponBeforeFired;
        event System.Action<IWeapon> OnWeaponFired;

        GameObject ProjectilePrefab { get; set; }
        GameObject DamageOwner { get; set; }
        Transform BulletSpawn { get; set; }
        bool IsOutOfAmmo { get; set; }

        void Fire();
        bool CanFire();
        void Upgrade();
    }
}
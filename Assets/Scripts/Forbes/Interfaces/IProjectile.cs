using UnityEngine;

namespace Forbes
{
    public interface IProjectile
    {
        GameObject DamageOwner { get; set; }
        GameObject Target { get; set; }
    }
}
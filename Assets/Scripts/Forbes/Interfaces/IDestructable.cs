using UnityEngine;

namespace Forbes.Damage
{
    public interface IDestructable
    {
        void Heal(int amount);
        void TakeDamage(int amount);
    }
}
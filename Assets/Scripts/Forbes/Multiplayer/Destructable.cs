using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using UnityEngine.UI;

namespace Forbes.Multiplayer
{
    [RequireComponent(typeof(NetworkObject))]
    public class Destructable : NetworkBehaviour, Forbes.Damage.IDestructable
    {
        NetworkVariableInt Health = new NetworkVariableInt(100);

        public Text UIHealth;

        void OnHealth(int oldValue, int newValue)
        {
            UIHealth.text = newValue.ToString();
            if (Health.Value <= 0)
                GameManager.Spawner.Despawn(gameObject);
        }

        void OnEnable()
        {
            Health.Value = 100;
            Health.OnValueChanged += OnHealth;

            if (IsClient && IsOwner)
                UIHealth.text = Health.Value.ToString();
        }

        void OnDisable()
        {
            Health.OnValueChanged -= OnHealth;
        }

        private void FixedUpdate()
        {
            if (Health.Value <= 0)
                return;

            if (!IsServer)
                return;

            if (IsLocalPlayer)
                if (Forbes.SinglePlayer.GameManager.Instance.InputController.Key1)
                    this.TakeDamage(50);
        }

        public void TakeDamage(int amount)
        {
            if (!IsServer) return;

            Health.Value -= amount;
        }

        public void Heal(int amount)
        {
            if (!IsServer) return;

            Health.Value += amount;
        }
    }
}
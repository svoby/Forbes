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

        public Text Text;

        void OnHealth(int oldValue, int newValue)
        {
            Text.text = newValue.ToString();
            TakeDamage(oldValue - newValue);
        }

        void OnEnable()
        {
            Health.Value = 100;
            Health.OnValueChanged += OnHealth;

            if (IsClient && IsOwner)
                Text.text = Health.Value.ToString();
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
                    Health.Value -= 50;
        }

        public void TakeDamage(int amount)
        {
            if (Health.Value <= 0)
                Forbes.SinglePlayer.GameManager.Spawner.Despawn(gameObject);
        }

        public void Heal(int amount)
        {
            Health.Value += amount;
        }
    }
}
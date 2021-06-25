using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.NetworkVariable;

namespace Forbes.Multiplayer
{
    [RequireComponent(typeof(NetworkObject))]
    public class Destructable : NetworkBehaviour, Forbes.Damage.IDestructable
    {
        NetworkVariableInt Health = new NetworkVariableInt(100);

        public Text UIHealth;

        void OnHealth(int oldValue, int newValue)
        {
            if (IsLocalPlayer)
                UIHealth.text = newValue.ToString();

            if (Health.Value <= 0 && IsServer)
                GameManager.Spawner.Despawn(gameObject);
        }

        void OnEnable()
        {
            Health.Value = 100;
            Health.OnValueChanged += OnHealth;

            if (IsLocalPlayer)
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

            if (IsServer && IsLocalPlayer)
            {
                if (Forbes.SinglePlayer.GameManager.Instance.InputController.Key1)
                    this.TakeDamage(100);

                if (Forbes.SinglePlayer.GameManager.Instance.InputController.Key2)
                    this.KillAllClients();
            }
        }

        private void KillAllClients()
        {
            var list = NetworkManager.Singleton.ConnectedClientsList;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ClientId == NetworkManager.Singleton.LocalClientId)
                    continue;

                GameObject go = list[i].PlayerObject.gameObject;
                GameManager.Spawner.Despawn(go);
            }
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
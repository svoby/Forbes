using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Forbes.Spawning
{
    public class SpawnSelf : MonoBehaviour, ISpawn
    {
        public event Action<GameObject> OnSpawn;
        public event Action<GameObject> OnDespawn;

        [Header("OnSpawn")]
        public AudioClip SoundSpawn;
        [SerializeField] List<GameObject> OnSpawnList;
        [SerializeField] float SpawnPropability;
        [SerializeField] bool SpawnRandomize;

        [Header("OnDespawn")]
        public AudioClip SoundDespawn;
        [SerializeField] List<GameObject> OnDespawnList;
        [SerializeField] [Range(0, 1f)] float DespawnPropability = 1f;
        [SerializeField] bool DespawnRandomize;

        public void Spawn()
        {
            foreach (var o in OnSpawnList)
                if (o != null)
                    GameManager.Instance.Spawner.Spawn(o, transform.position, transform.rotation);

            if (SoundSpawn != null)
                AudioSource.PlayClipAtPoint(SoundSpawn, transform.position);

            if (OnSpawn != null)
                OnSpawn(gameObject);
        }

        public void Despawn()
        {
            if (OnDespawnList == null)
                return;

            if (DespawnPropability < UnityEngine.Random.Range(0, 1f))
                return;

            var list = OnDespawnList.Where(i => i != null);
            if (list.Count() == 0)
                return;

            if (DespawnRandomize)
            {
                GameManager.Instance.Spawner.Spawn(OnDespawnList[UnityEngine.Random.Range(0, list.Count())], transform.position, transform.rotation);
            }
            else
            {
                foreach (var o in list)
                    GameManager.Instance.Spawner.Spawn(o, transform.position, transform.rotation);
            }

            if (SoundDespawn != null)
                AudioSource.PlayClipAtPoint(SoundDespawn, transform.position);

            if (OnDespawn != null)
                OnDespawn(gameObject);
        }
    }
}
﻿using System;
using UnityEngine;

public class DoomPickupAmmoShotgun : Pickup, ISpawn
{
    public event Action<GameObject> OnSpawn;
    public event Action<GameObject> OnDespawn;

    void Awake()
    {
        OnPickedUp += (GameObject go) => GameManager.Instance.GameLogic.ItemPickedUp(this.gameObject, go);
        OnSpawn += (GameObject go) => GetComponent<PhysicX>()?.AddForce(0f, 2f, 0);
    }

    public void Spawn()
    {
        if (OnSpawn != null)
            OnSpawn(gameObject);
    }
    public void Despawn()
    {
        if (OnDespawn != null)
            OnDespawn(gameObject);
    }
}
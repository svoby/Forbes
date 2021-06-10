using UnityEngine;

public interface IProjectile
{
	GameObject DamageOwner { get; set; }
	GameObject Target { get; set; }
}
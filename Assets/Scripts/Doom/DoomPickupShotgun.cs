using UnityEngine;

public class DoomPickupShotgun : Pickup
{
	void Awake()
	{
		OnPickedUp += (GameObject go) => GameManager.Instance.GameLogic.ItemPickedUp(this.gameObject, go);
	}
}
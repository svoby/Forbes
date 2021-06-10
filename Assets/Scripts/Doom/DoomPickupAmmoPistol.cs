using UnityEngine;

public class DoomPickupAmmoPistol : Pickup
{
	void Awake()
	{
		OnPickedUp += (GameObject go) => GameManager.Instance.GameLogic.ItemPickedUp(this.gameObject, go);
	}
}
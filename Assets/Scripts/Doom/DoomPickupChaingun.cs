using UnityEngine;

public class DoomPickupChaingun : Pickup
{
	void Awake()
	{
		OnPickedUp += (GameObject go) => GameManager.Instance.GameLogic.ItemPickedUp(this.gameObject, go);
	}
}
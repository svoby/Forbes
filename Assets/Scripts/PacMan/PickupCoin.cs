using UnityEngine;

public class PickupCoin : Pickup
{
	void Awake()
	{
		if (GameManager.Instance.GameLogic != null)
			OnPickedUp += (GameObject go) => GameManager.Instance.GameLogic.ItemPickedUp(this.gameObject, go);
	}
}
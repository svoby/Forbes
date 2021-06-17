namespace Pistacio.PacMan
{
    using UnityEngine;

    public class PickupCoin : Forbes.Pickup
    {
        private void Awake() {
            OnPickedUp += (GameObject go) => Debug.Log("PacMan coin picked up!");
        }
    }
}
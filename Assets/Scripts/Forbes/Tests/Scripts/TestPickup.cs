using UnityEngine;

namespace Forbes.Tests
{
    public class TestPickup : Forbes.SinglePlayer.Pickup
    {
        private void Awake() {
            OnPickedUp += (GameObject go) => Debug.Log("Singleplayer TestPickup picked up!");
        }
    }
}
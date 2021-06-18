namespace Forbes.Tests
{
    using UnityEngine;

    public class TestPickup : Pickup
    {
        private void Awake() {
            OnPickedUp += (GameObject go) => Debug.Log("TestPickup picked up!");
        }
    }
}
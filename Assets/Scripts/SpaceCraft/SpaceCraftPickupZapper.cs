using UnityEngine;

public class SpaceCraftPickupZapper : Pickup
{
    void Awake()
    {
        OnPickedUp += (GameObject go) =>
        {
            GameManager.Instance.LocalPlayer.MC.SetWeapon(0);
            GameManager.Instance.LocalPlayer.MC.GetComponent<SpaceCraftAnimator>().HasWeapon = true;
        };
    }
}
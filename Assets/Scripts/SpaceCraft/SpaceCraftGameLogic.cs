using UnityEngine.UI;
using UnityEngine;
using System;

public class SpaceCraftGameLogic : MonoBehaviour, IGameLogic
{
    IWeapon m_ActiveWeapon;

    Container m_Inventory;
    public Container Inventory
    {
        get {
            if (m_Inventory == null)
                m_Inventory = new Container();
            return m_Inventory;
        }
    }
    private int m_AmmoPistolMax = 100;
    private int m_AmmoShotgunMax = 50;

    public Text UIMinerals;
    public Text UIWeapons;
    public Text UIMainText;
    public Text UIHealth;

    public int DefaultWeaponIndex = -1;

    void Awake()
    {
        GameManager.Instance.OnLocalPlayerJoined += (IPlayerController go) =>
        {
            Inventory.Add("ammoPistol", 20, m_AmmoPistolMax);

            go.MC.OnWeaponRegistered += (IWeapon weapon) => weapon.OnWeaponBeforeFired += this.OnWeaponBeforeFired;

            if (DefaultWeaponIndex > -1)
                go.MC.OnAllWeaponsRegistered += () => GameManager.Instance.LocalPlayer.MC.SetWeapon(DefaultWeaponIndex);

            GameManager.Instance.LocalPlayer.MC.GetComponent<Destructable>().OnDamageReceived += () => this.UIUpdateHealth();
            GameManager.Instance.LocalPlayer.MC.GetComponent<Destructable>().OnDeath += () => this.GameOver();

            UIUpdateMainText("LEVEL 1:\nRepair The Ship", 5f);
            this.UIUpdateHealth();
            this.UIUpdate();
        };
    }

    void Update()
    {
        if (GameManager.Instance.InputController.State.Key1)
            GameManager.Instance.LocalPlayer.MC.SetWeapon(0);

        if (GameManager.Instance.InputController.State.Key2 && Inventory.Contains("shotgun") != null)
            GameManager.Instance.LocalPlayer.MC.SetWeapon(1);
    }

    void OnWeaponBeforeFired(IWeapon weapon)
    {
        switch (weapon)
        {
            case SpaceCraftWeaponZapper zapper:
                if (Inventory.Get("ammoPistol", 1) <= 0)
                {
                    GameManager.Instance.LocalPlayer.MC.GetWeapon(0).OutOfAmmo = true;
                    this.UIUpdateMainText("Out Off Zapper Ammo", 3f);
                }
                break;
            case DoomWeaponShotgun shotgun:
                if (Inventory.Get("ammoShotgun", 1) <= 0)
                {
                    GameManager.Instance.LocalPlayer.MC.GetWeapon(1).OutOfAmmo = true;
                    this.UIUpdateMainText("Out Off Shotgun Ammo", 3f);
                }
                break;
        }

        this.UIUpdate();
    }

    void GameOver()
    {
        UIUpdateMainText("GAME OVER!", 3f);
    }

    public void ItemPickedUp(GameObject _item, GameObject _by)
    {
        IPickup ip = _item.GetComponent<IPickup>();

        switch (ip)
        {
            case DoomPickupAmmoPistol ammo:
                Inventory.Add("ammoPistol", 50, m_AmmoPistolMax);
                GameManager.Instance.LocalPlayer.MC.GetComponent<SpaceCraftAnimator>().HasWeapon = true;
                GameManager.Instance.LocalPlayer.MC.GetWeapon(0).OutOfAmmo = false;
                UIUpdateMainText("+Zapper ammo", 3f);
                break;

            case DoomPickupAmmoShotgun ammo:
                Inventory.Add("ammoShotgun", 20, m_AmmoShotgunMax);
                GameManager.Instance.LocalPlayer.MC.GetWeapon(1).OutOfAmmo = false;
                UIUpdateMainText("+20 Shotgun shells", 3f);
                break;

            case DoomPickupShotgun shotgun:
                Inventory.AddOnce("shotgun");
                Inventory.Add("ammoShotgun", 10, m_AmmoShotgunMax);
                GameManager.Instance.LocalPlayer.MC.GetComponent<SpaceCraftAnimator>().HasWeapon = true;
                GameManager.Instance.LocalPlayer.MC.SetWeapon(1).OutOfAmmo = false;
                UIUpdateMainText("+Shotgun picked up", 3f);
                break;

            case SpaceCraftPickupMineral mineral1:
                Inventory.Add("minerals", 1, 100);
                UIUpdateMainText("+1 Bule mineral", 3f);
                break;

            case SpaceCraftPickupUpgrade upgrade :
                UIUpdateMainText("Weapon upgraded!", 3f);
                GameManager.Instance.LocalPlayer.MC.ActiveWeapon.Upgrade();
                break;
        }

        this.UIUpdate();
    }

    void UIUpdate()
    {
        this.UIUpdateWeapons();
        this.UIUpdateMinerals();
        this.UIUpdateHealth();
    }

    void UIUpdateHealth()
    {
        if (UIHealth == null) return;
        UIHealth.text = GameManager.Instance.LocalPlayer.MC.GetComponent<Destructable>().HitPointsRemaining.ToString();
    }

    void UIUpdateMainText(String text, float time = 0f)
    {
        if (UIMainText == null) return;

        UIMainText.text = text;
        UIMainText.enabled = true;

        if (time > 0)
            GameManager.Instance.Timer.Add(() => UIMainText.enabled = false, time);
    }

    void UIUpdateWeapons()
    {
        if (UIWeapons == null) return;

        UIWeapons.text = "1: Zapper";

        ContainerItem ammoPistol = Inventory.Contains("ammoPistol");
        ContainerItem ammoShotgun = Inventory.Contains("ammoShotgun");

        if (ammoPistol != null)
            UIWeapons.text += " (" + ammoPistol.Amount + "/" + m_AmmoPistolMax + ")";

        if (Inventory.Contains("shotgun") != null)
        {
            UIWeapons.text += "\n2: Shotgun";
            UIWeapons.text += " (" + ammoShotgun.Amount + "/" + m_AmmoShotgunMax + ")";
        }
    }

    void UIUpdateMinerals()
    {
        if (UIMinerals == null) return;

        int Minerals = 0;
        ContainerItem slot = Inventory.Contains("minerals");

        if (slot != null) Minerals = slot.Amount;

        UIMinerals.text = "Minerals: " + Minerals;
    }

    public void TargetKilled(GameObject _target, GameObject _by)
    {
        // // Player death
        // if (_target.name == GameManager.Instance.LocalPlayer.name)
        // {
        //     m_Destructable = _target.GetComponent<Destructable>();
        //     if (m_Destructable != null)
        //     {
        //         GameManager.Instance.Timer.Add(PlayerRespawn, 3f);
        //         UIUpdateMainText("Game over!", 5f);
        //     }
        // }
    }

    public void ItemSpawned(GameObject _item)
    {
    }

    public bool InteractWith(ICanInteract go)
    {
        bool response = false;
        switch (go)
        {
            case SpaceCraftShip ship:
                ContainerItem minerals = Inventory.Contains("minerals");
                if (minerals != null && minerals.Amount >= 5) {
                    response = true;
                    minerals.Get(5);
                    UIUpdateMainText("Ship Repaired.\nGood Job!", 5f);
                } else {
                    UIUpdateMainText("5 Blue Minerals\nNeeded", 5f);
                    response = false;
                }
                break;
        }

        return response;
    }
}

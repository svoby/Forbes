// using UnityEngine.UI;
// using UnityEngine;
// using System;

// public class DoomGameLogic : MonoBehaviour, IGameLogic
// {
//     int m_CoinsToPickuUp = 4;

//     IWeapon m_ActiveWeapon;

//     public Container Inventory = new Container();
//     private int m_AmmoPistolMax = 100;
//     private int m_AmmoShotgunMax = 50;

//     public Text UICoins;
//     public Text UIWeapons;
//     public Text UIMainText;
//     public Text UIHealth;
//     public Text UIGameOver;

//     public int DefaultWeaponIndex = -1;

//     void Awake()
//     {
//         GameManager.Instance.OnLocalPlayerJoined += (PlayerController go) =>
//         {
//             Inventory.Add("ammoPistol", 20, m_AmmoPistolMax);

//             go.MC.OnWeaponRegistered += (IWeapon weapon) => weapon.OnWeaponBeforeFired += this.OnWeaponBeforeFired;

//             if (DefaultWeaponIndex > -1)
//                 go.MC.OnAllWeaponsRegistered += () => GameManager.Instance.LocalPlayer.MC.SetWeapon(DefaultWeaponIndex);

//             GameManager.Instance.LocalPlayer.GetComponent<Destructable>().OnDamageReceived += () => this.UIUpdateHealth();
//             GameManager.Instance.LocalPlayer.GetComponent<Destructable>().OnDeath += () => this.GameOver();

//             this.UIUpdateHealth();
//             this.UIUpdate();
//         };
//     }

//     void Update()
//     {
//         if (GameManager.Instance.InputController.State.Key1)
//             GameManager.Instance.LocalPlayer.MC.SetWeapon(0);

//         if (GameManager.Instance.InputController.State.Key2 && Inventory.Contains("chaingun") != null)
//             GameManager.Instance.LocalPlayer.MC.SetWeapon(1);

//         if (GameManager.Instance.InputController.State.Key3 && Inventory.Contains("shotgun") != null)
//             GameManager.Instance.LocalPlayer.MC.SetWeapon(2);
//     }

//     void OnWeaponBeforeFired(IWeapon weapon)
//     {
//         switch (weapon)
//         {
//             case DoomWeaponPistol pistol:
//                 if (Inventory.Get("ammoPistol", 1) <= 0)
//                 {
//                     GameManager.Instance.LocalPlayer.MC.GetWeapon(0).OutOfAmmo = true;
//                     GameManager.Instance.LocalPlayer.MC.GetWeapon(1).OutOfAmmo = true;
//                     this.UIUpdateMainText("Out Off Pistol Ammo", 3f);
//                 }
//             break;
//             case DoomWeaponShotgun shotgun:
//                 if (Inventory.Get("ammoShotgun", 1) <= 0)
//                 {
//                     GameManager.Instance.LocalPlayer.MC.GetWeapon(2).OutOfAmmo = true;
//                     this.UIUpdateMainText("Out Off Shotgun Ammo", 3f);
//                 }
//             break;
//         }

//         this.UIUpdate();
//     }

//     void GameOver()
//     {
//         GameManager.Instance.Timer.Add(PlayerRespawn, 3f);
//     }

//     public void ItemPickedUp(GameObject _item, GameObject _by)
//     {
//         IPickup ip = _item.GetComponent<IPickup>();

//         switch (ip)
//         {
//             case DoomPickupChaingun chaingun:
//                 Inventory.AddOnce("chaingun");
//                 Inventory.Add("ammoPistol", 30, m_AmmoPistolMax);
//                 GameManager.Instance.LocalPlayer.MC.GetWeapon(0).OutOfAmmo = false;
//                 GameManager.Instance.LocalPlayer.MC.SetWeapon(1).OutOfAmmo = false;
//                 UIUpdateMainText("+Chaingun", 2f);
//                 break;

//             case DoomPickupShotgun shotgun:
//                 Inventory.AddOnce("shotgun");
//                 Inventory.Add("ammoShotgun", 10, m_AmmoShotgunMax);
//                 GameManager.Instance.LocalPlayer.MC.SetWeapon(2).OutOfAmmo = false;
//                 UIUpdateMainText("+Shotgun", 2f);
//                 break;

//             case DoomPickupAmmoPistol ammo:
//                 Inventory.Add("ammoPistol", 50, m_AmmoPistolMax);
//                 GameManager.Instance.LocalPlayer.MC.GetWeapon(0).OutOfAmmo = false;
//                 GameManager.Instance.LocalPlayer.MC.GetWeapon(1).OutOfAmmo = false;
//                 UIUpdateMainText("+Ammo", 2f);
//                 break;

//             case DoomPickupAmmoShotgun ammo:
//                 Inventory.Add("ammoShotgun", 20, m_AmmoShotgunMax);
//                 GameManager.Instance.LocalPlayer.MC.GetWeapon(2).OutOfAmmo = false;
//                 UIUpdateMainText("+20 Shotgun ammo", 2f);
//                 break;

//             case PickupCoin coin:
//                 Inventory.Add("coins", 1, 100);
//                 UIUpdateMainText("+1 Coin", 2f);
//                 break;

//             case PickupHealth health:
//                 GameManager.Instance.LocalPlayer.GetComponent<Destructable>().Heal(50);
//                 UIUpdateMainText("+50 heal", 2f);
//                 break;
//         }

//         this.UIUpdate();
//     }

//     void UIUpdate()
//     {
//         this.UIUpdateWeapons();
//         this.UIUpdateCoins();
//         this.UIUpdateHealth();
//     }

//     void UIUpdateHealth()
//     {
//         if (UIHealth == null) return;
//         UIHealth.text = GameManager.Instance.LocalPlayer.GetComponent<Destructable>().HitPointsRemaining.ToString();
//     }

//     void UIUpdateMainText(String text, float time = 0f)
//     {
//         if (UIMainText == null) return;

//         UIMainText.text = text;
//         UIMainText.enabled = true;

//         if (time > 0)
//             GameManager.Instance.Timer.Add(() => UIMainText.enabled = false, time);
//     }

//     void UIUpdateWeapons()
//     {
//         if (UIWeapons == null) return;

//         UIWeapons.text = "1: Pistol";

//         ContainerItem ammoPistol = Inventory.Contains("ammoPistol");
//         ContainerItem ammoShotgun = Inventory.Contains("ammoShotgun");

//         if (ammoPistol != null)
//             UIWeapons.text += " (" + ammoPistol.Amount + "/" + m_AmmoPistolMax + ")";

//         if (Inventory.Contains("chaingun") != null)
//         {
//             UIWeapons.text += "\n2: Chaingun";
//             UIWeapons.text += " (" + ammoPistol.Amount + "/" + m_AmmoPistolMax + ")";
//         }

//         if (Inventory.Contains("shotgun") != null)
//         {
//             UIWeapons.text += "\n3: Shotgun";
//             UIWeapons.text += " (" + ammoShotgun.Amount + "/" + m_AmmoShotgunMax + ")";
//         }
//     }

//     void UIUpdateCoins()
//     {
//         if (UICoins == null) return;

//         int coins = 0;
//         ContainerItem slot = Inventory.Contains("coins");

//         if (slot != null) coins = slot.Amount;

//         UICoins.text = "Coins: " + coins + " / " + m_CoinsToPickuUp;
//     }

//     public void TargetKilled(GameObject _target, GameObject _by)
//     {
//         // // Player death
//         // if (_target.name == GameManager.Instance.LocalPlayer.name)
//         // {
//         //     m_Destructable = _target.GetComponent<Destructable>();
//         //     if (m_Destructable != null)
//         //     {
//         //         GameManager.Instance.Timer.Add(PlayerRespawn, 3f);
//         //         UIUpdateMainText("Game over!", 5f);
//         //     }
//         // }
//     }

//     public void SpawnItem(GameObject _item)
//     {
//         // Coin Spawn
//         if (_item.GetComponent<PickupCoin>())
//             m_CoinsToPickuUp++;
//     }

//     void PlayerRespawn()
//     {
//         //GameManager.Instance.LocalPlayer.GetComponent<Destructable>().Reset();
//     }

//     bool IGameLogic.InteractWith(GameObject go)
//     {
//         throw new NotImplementedException();
//     }
// }

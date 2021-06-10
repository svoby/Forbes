// using UnityEngine.UI;
// using UnityEngine;

// public class PacManGameLogic : MonoBehaviour, IGameLogic
// {
//     int m_CoinsToPickuUp = 0;
//     int m_CoinsPickedUp = 0;

//     Destructable m_Destructable;
//     public Text CoinsUIText;

//     void Start()
//     {
//         CoinsUIText.text = "Coins: " + m_CoinsPickedUp + " / " + m_CoinsToPickuUp;
//     }

//     public void ItemPickedUp(GameObject _item, GameObject _by)
//     {
//         if (_item.GetComponent<PickupCoin>())
//         {
//             m_CoinsPickedUp++;
//             CoinsUIText.text = "Coins: " + m_CoinsPickedUp + " / " + m_CoinsToPickuUp;

//             if (m_CoinsPickedUp >= m_CoinsToPickuUp)
//             {
//                 Debug.Log("**************** Player Won ******");
//             }
//         }
//     }

//     public void TargetKilled(GameObject _target, GameObject _by)
//     {
//         // Player death
//         if (_target.name == GameManager.Instance.LocalPlayer.name)
//         {
//             m_Destructable = _target.GetComponent<Destructable>();
//             if (m_Destructable != null)
//                 GameManager.Instance.Timer.Add(PlayerRespawn, 3f);
//         }
//     }

//     public void SpawnItem(GameObject _item)
//     {
//         // Coin Spawn
//         if (_item.GetComponent<PickupCoin>())
//             m_CoinsToPickuUp++;
//     }

//     void PlayerRespawn()
//     {
//         //m_Destructable.Reset();
//     }

//     public bool InteractWith(GameObject go)
//     {
//         throw new System.NotImplementedException();
//     }
// }

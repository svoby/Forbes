// using UnityEngine;

// public class WowGameLogic : MonoBehaviour, IGameLogic
// {
//     public int m_CoinsToPickuUp = 5;
//     int m_CoinsPickedUp = 0;

//     public int m_EnemiesToKill = 2;
//     int m_EnemiesKiled = 0;

//     public void ItemPickedUp(GameObject _go, GameObject _by)
//     {
//         if (_go.GetComponent<PickupCoin>())
//         {
//             m_CoinsPickedUp++;
//             Debug.Log("Coins: " + m_CoinsPickedUp + "/" + m_CoinsToPickuUp);
//             if (m_CoinsPickedUp >= m_CoinsToPickuUp)
//                 Debug.Log("Player won by coins!");
//         }
//     }

//     public void TargetKilled(GameObject _target, GameObject _by)
//     {
//         Debug.Log("Target " + _target.name + " killed.");
//         m_EnemiesKiled++;

//         if (m_EnemiesKiled >= m_EnemiesToKill) {
//             Debug.Log("Player won by killing!");
//         }
//     }

//     public void SpawnItem(GameObject _item)
//     {
//         throw new System.NotImplementedException();
//     }

//     public bool InteractWith(GameObject go)
//     {
//         throw new System.NotImplementedException();
//     }
// }
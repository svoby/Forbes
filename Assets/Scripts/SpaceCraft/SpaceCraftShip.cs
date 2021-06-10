using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanInteract
{

}

public class SpaceCraftShip : MonoBehaviour, ICanInteract
{
    bool m_PlayerIn = false;
    public Transform PlayerBody;
    public Transform ShipBody;
    public Animator ShipAnimator;
    bool m_CanJumpInOut = false;
    public bool m_IsDamaged = true;
    
    private void OnEnable()
    {
        GameManager.Instance.Timer.Add(() => m_CanJumpInOut = true, 1f);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (m_CanJumpInOut && GameManager.Instance.InputController.State.Strafe == 1)
        {
            if (!m_PlayerIn && m_IsDamaged)
            {
                if (GameManager.Instance.GameLogic.InteractWith(this))
                {
                    GameManager.Instance.Spawner.Despawn(gameObject);
                    return;
                }
                else
                {
                    m_IsDamaged = true;
                }
            }

            if (!m_PlayerIn && !m_IsDamaged)
            {
                GameManager.Instance.LocalPlayer.MC.PhysicX.MoveTo(transform.position);
                transform.parent = PlayerBody;
                transform.rotation = PlayerBody.rotation;
                transform.localPosition = new Vector3(0, 0, 0);
                m_PlayerIn = true;

                PlayerBody.parent.transform.parent.GetComponent<SpaceCraftAnimator>().IsInShip = true;
                ShipAnimator.SetBool("IsInShip", true);
            }
            else if (!m_IsDamaged)
            {
                transform.parent = null;
                PlayerBody.parent.transform.parent.GetComponent<SpaceCraftAnimator>().IsInShip = false;
                ShipAnimator.SetBool("IsInShip", false);
                m_PlayerIn = false;
            }

            m_CanJumpInOut = false;
            GameManager.Instance.Timer.Add(() => m_CanJumpInOut = true, 1f);
        }
    }
}

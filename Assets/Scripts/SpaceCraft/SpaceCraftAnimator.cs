using UnityEngine;

public class SpaceCraftAnimator : MonoBehaviour
{
    public Animator Animator;
    public PhysicX PhysicX;
    public bool IsInShip;
    public bool HasWeapon;

    void FixedUpdate()
    {
        if (Animator == null || PhysicX == null)
            return;

        if (IsInShip) {
            Animator.SetBool("IsWalking", false);
            Animator.SetBool("IsInShip", true);
            return;
        } else {
            Animator.SetBool("IsWalking", false);
            Animator.SetBool("IsInShip", false);
        }

        float magnitude = new Vector2(PhysicX.V.x, PhysicX.V.z).magnitude;
        if (magnitude >= 1f) {
            Animator.SetBool("IsWalking", true);
        } else {
            Animator.SetBool("IsWalking", false);
        }

        if (!PhysicX.IsGrounded && !IsInShip && Mathf.Abs(PhysicX.V.y) > 3f) {
            Animator.SetBool("IsWalking", false);
            Animator.SetBool("IsInShip", false);
            Animator.SetBool("IsInAir", true);
        } else {
            Animator.SetBool("IsInAir", false);
        }

        if (HasWeapon)
            Animator.SetBool("HasWeapon", true);
    }
}

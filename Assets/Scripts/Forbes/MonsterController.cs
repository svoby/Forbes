using System.Collections.Generic;
using UnityEngine;
using Forbes.Inputs;

namespace Forbes
{
    [RequireComponent(typeof(PhysicX))]
    [RequireComponent(typeof(CharacterController))]
    public class MonsterController : MonoBehaviour
    {
        public event System.Action<IWeapon> OnWeaponRegistered;
        public event System.Action OnAllWeaponsRegistered;
        public event System.Action<InputFrame> OnMoved;

        #region Variables

        [Header("Speeds")]
        public float WalkSpeed = 4f;
        public float RunSpeedMultipler = 1f;

        [Header("Transforms")]
        public GameObject Body;
        public GameObject Aiming;

        [Header("Sounds")]
        public AudioClip SoundJump;

        [Header("Rotate")]
        public float RotateLerp = 0.3f;

        PhysicX m_PhysicX;
        public PhysicX PhysicX
        {
            get
            {
                if (m_PhysicX == null)
                    m_PhysicX = gameObject.GetComponent<PhysicX>();
                return m_PhysicX;
            }
        }

        [Header("Weapons")]
        public Transform WeaponsTransform;

        public IWeapon ActiveWeapon;

        public class WeaponContainer
        {
            public GameObject parentGo;
            public IWeapon weapon;
        }

        List<WeaponContainer> m_WeaponList;
        public List<WeaponContainer> WeaponList
        {
            get
            {
                if (m_WeaponList == null)
                    m_WeaponList = this.RegisterWeapons();

                return m_WeaponList;
            }
            set
            {
                m_WeaponList = value;
            }
        }

        float m_LastTimeFired;
        float m_LastTimeFiredDelay = 2f;

        #endregion

        public void ApplyInputs(InputFrame input, InputMask mask = 0)
        {
            Move(input);
            Jump(input, mask);
            Rotate(input, mask);
            Fire(input, mask);
        }

        protected void Move(InputFrame input)
        {
            if (input.Horizontal != 0 || input.Vertical != 0)
            {
                Vector2 clampedXZ = Vector2.ClampMagnitude(new Vector2(input.Horizontal, input.Vertical), 1);
                Vector3 _v = Vector3.zero;

                _v.x = clampedXZ.x * WalkSpeed * RunSpeedMultipler;
                _v.z = clampedXZ.y * WalkSpeed * RunSpeedMultipler;

                float magnitude = new Vector2(PhysicX.V.x, PhysicX.V.z).magnitude;
                if (magnitude < (WalkSpeed * RunSpeedMultipler))
                {
                    float delta = WalkSpeed * RunSpeedMultipler - magnitude;
                    _v = Vector3.ClampMagnitude(_v, delta);
                }
                else
                {
                    _v = Vector3.zero;
                }

                PhysicX.AddForce(_v);

                if (OnMoved != null)
                    OnMoved(input);
            }
        }

        protected void Jump(InputFrame input, InputMask mask = 0)
        {
            if (input.Jump)
            {
                PhysicX.V.y = 0;
                PhysicX.AddForce(new Vector3(0, Mathf.Sqrt(-2f * Physics.gravity.y * PhysicX.JumpHeight * PhysicX.Mass * 1 / PhysicX.LerpStep), 0));
                PhysicX.IsGrounded = false;
                PhysicX.SkipNextGroundTest = true;

                if (!mask.HasFlag(InputMask.SilenceJump) && SoundJump != null)
                    AudioSource.PlayClipAtPoint(SoundJump, transform.position);
            }
        }

        public void Rotate(InputFrame input, InputMask mask = 0)
        {
            // Rotate aiming
            if (Aiming != null && input.Rotation != 0)
            {
                Vector3 aimAngles = Aiming.transform.rotation.eulerAngles;
                Aiming.transform.rotation = Quaternion.Euler(aimAngles.x, input.Rotation, aimAngles.z);
            }

            if (Body != null)
            {
                if (input.Fire != 0 || mask.HasFlag(InputMask.ForceBodyRotate) || input.Mask.HasFlag(InputMask.ForceBodyRotate) || m_LastTimeFired < m_LastTimeFiredDelay)
                {
                    Body.transform.forward = Aiming.transform.forward;
                }

                if (mask.HasFlag(InputMask.SilenceRotate))
                {
                    Body.transform.forward = Vector3.Lerp(Body.transform.forward, Aiming.transform.forward, RotateLerp);
                }

                if (input.Fire == 0 && (PhysicX.V.x + PhysicX.V.z) != 0 && m_LastTimeFired > m_LastTimeFiredDelay)
                {
                    Body.transform.forward = Vector3.Lerp(Body.transform.forward, new Vector3(PhysicX.V.x, 0, PhysicX.V.z).normalized, RotateLerp);
                }
            }
        }

        protected virtual void Fire(InputFrame input, InputMask mask = 0)
        {
            m_LastTimeFired += Time.fixedDeltaTime;

            if (input.Fire == 0 || mask.HasFlag(InputMask.SilenceFire))
                return;

            if (ActiveWeapon != null)
            {
                ActiveWeapon.Fire();
                m_LastTimeFired = 0;
            }

        }

        public List<WeaponContainer> RegisterWeapons()
        {
            if (WeaponsTransform == null)
                return null;

            WeaponList = new List<WeaponContainer>();

            foreach (Transform child in WeaponsTransform)
            {
                IWeapon weapon = child.GetComponent<IWeapon>();
                weapon.DamageOwner = gameObject;

                WeaponList.Add(new WeaponContainer
                {
                    parentGo = child.gameObject,
                    weapon = weapon
                });

                if (OnWeaponRegistered != null)
                    OnWeaponRegistered(weapon);
            }

            if (OnAllWeaponsRegistered != null)
                OnAllWeaponsRegistered();

            return WeaponList;
        }

        internal IWeapon SetWeapon(int index)
        {
            if (WeaponsTransform == null)
                return null;

            int i = 0;
            foreach (WeaponContainer weaponContainer in WeaponList)
            {
                if (index == i)
                {
                    weaponContainer.parentGo.SetActive(true);
                    ActiveWeapon = weaponContainer.weapon;
                }
                else
                {
                    weaponContainer.parentGo.SetActive(false);
                }

                i++;
            }

            return ActiveWeapon;
        }

        internal IWeapon GetWeapon(int index)
        {
            int i = 0;
            foreach (WeaponContainer weaponContainer in WeaponList)
            {
                if (index == i)
                    return weaponContainer.weapon;
                i++;
            }

            return null;
        }

    }
}
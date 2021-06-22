using UnityEngine;

namespace Forbes
{
    public class PhysicX : MonoBehaviour
    {
        public Vector3 V;
        [SerializeField] Vector3 Drag = new Vector3(5, 0, 5);
        public float Mass = 1;
        [SerializeField] float GroundHeight = 1f;
        public float JumpHeight = 2f;
        public bool UseLerp = false;
        public float LerpStep = 1f;
        public bool Bounce = false;

        CharacterController m_CC;
        public CharacterController CC
        {
            get
            {
                if (m_CC == null)
                    m_CC = GetComponent<CharacterController>();
                return m_CC;
            }
        }

        private bool m_IsGrounded;
        public bool IsGrounded { get; set; }
        public bool SkipNextStep { get; set; }
        public bool SkipNextGroundTest { get; set; }

        float lastVy;

        private void OnEnable()
        {
            V = Vector3.zero;
        }

        void Start()
        {
            SkipNextStep = false;
        }

        void FixedUpdate()
        {
            Simulate(Time.deltaTime);
        }

        public void Simulate(float dt)
        {
            if (SkipNextStep)
            {
                SkipNextStep = false;
                return;
            }

            if (SkipNextGroundTest)
            {
                V.y += Mass * Physics.gravity.y * dt;
                SkipNextGroundTest = false;
            }
            else
            {
                if (IsGrounded)
                {
                    if (UseLerp)
                    {
                        V.y = 0; // Propably produces bug (with bounce)
                    }
                    else
                    {
                        V.y = Physics.gravity.y * dt;
                    }
                }
                else
                {
                    V.y += Mass * Physics.gravity.y * dt;
                }
            }

            // Drag
            V.x /= 1 + Drag.x * dt;
            V.y /= 1 + Drag.y * dt;
            V.z /= 1 + Drag.z * dt;

            if (Bounce)
                V.y = (IsGrounded && V.y <= -0.1f) ? -(V.y / 3) + Mass * Physics.gravity.y * dt : V.y; // TOD - nula?

            if (UseLerp)
            {
                if (CC != null) CC.enabled = false;
                transform.position = Vector3.Lerp(transform.position, transform.position + V * dt, LerpStep);
                if (CC != null) CC.enabled = true;
            }
            else
            {
                CC.Move(V * LerpStep * dt);
            }

            if (Mathf.Abs(V.x) < 0.1f)
                V.x = 0;

            if (Mathf.Abs(V.z) < 0.1f)
                V.z = 0;

            IsGrounded = !UseLerp ? CC.isGrounded : Physics.CheckSphere(transform.position, GroundHeight, LayerMask.GetMask("Floor"), QueryTriggerInteraction.Ignore);
        }

        public void AddForce(Vector3 force)
        {
            V += force;
        }

        public void AddForce(float x, float y, float z)
        {
            V += new Vector3(x, y, z);
        }

        public void MoveTo(Vector3 position)
        {
            if (UseLerp)
            {
                transform.position = position;
            }
            else
            {
                CC.enabled = false;
                transform.position = position;
                CC.enabled = true;
            }
        }

        public void ResetForces()
        {
            V = Vector3.zero;
        }
    }
}
using UnityEngine;
using System.Collections;
using LilyPadsEndlessJumper.PadBehaviours;

namespace LilyPadsEndlessJumper
{
    [RequireComponent(typeof(Rigidbody))]
    public class SlowVelocityStop : MonoBehaviour
    {
        public static event System.Action<BasicBehaviour> OnWaterLanded;
        public static event System.Action<BasicBehaviour> OnTargetLanded;
        public static event System.Action<BasicBehaviour> OnBullseyeLanded;
        public static event System.Action<BasicBehaviour> OnLanded;

        [SerializeField]
        float m_StopVelocity = 0.5f;
        [SerializeField]
        float m_DownCastDistance = 0.7f;
        [SerializeField]
        bool m_IsGrounded = false;
        Rigidbody m_RigidBody = null;

        public float speed = 0.0f;

        public bool isGrounded { get { return m_IsGrounded; } }

        public bool hasStopped { get; private set; }
        TargetGenerator m_TargetGenerator = null;
        void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_TargetGenerator = FindObjectOfType<TargetGenerator>();
        }

        bool m_AnyStop = false;
        bool m_WaterStop = false;
        bool m_TargetStop = false;
        bool m_BullseyeStop = false;

        void Update()
        {
            hasStopped = false;
            speed = m_RigidBody.velocity.magnitude;
            RaycastHit hit;
            m_IsGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, m_DownCastDistance);
            if (m_RigidBody.velocity.sqrMagnitude != 0.0f)
            {
                if (m_RigidBody.velocity.magnitude <= m_StopVelocity)
                {
                    if (m_IsGrounded)
                    {
                        hasStopped = true;
                        m_RigidBody.velocity = Vector3.zero;
                        m_RigidBody.angularVelocity = Vector3.zero;

                        if(!m_AnyStop)
                        {
                            m_AnyStop = true;
                            if (OnLanded != null)
                            {
                                BasicBehaviour basicBehaviour = hit.collider.GetComponent<BasicBehaviour>();
                                if (!basicBehaviour) basicBehaviour = hit.collider.GetComponentInParent<BasicBehaviour>();
                                if (!basicBehaviour) basicBehaviour = m_TargetGenerator.currentPadBehaviour;
                                OnLanded.Invoke(basicBehaviour);
                            }
                        }
                        if (!m_WaterStop && hit.collider.CompareTag(Tags.GROUND))
                        {
                            m_WaterStop = true;
                            if (OnWaterLanded != null)
                            {
                                OnWaterLanded.Invoke(m_TargetGenerator.nextPadBehaviour);
                            }
                        }
                        else if (!m_BullseyeStop && hit.collider.CompareTag(Tags.BULLSEYE_TARGET))
                        {
                            m_BullseyeStop = true;
                            if (OnBullseyeLanded != null)
                            {
                                BasicBehaviour basicBehaviour = hit.collider.GetComponent<BasicBehaviour>();
                                if (!basicBehaviour) basicBehaviour = hit.collider.GetComponentInParent<BasicBehaviour>();
                                OnBullseyeLanded.Invoke(basicBehaviour);
                            }
                        }
                        else if (!m_TargetStop && hit.collider.CompareTag(Tags.TARGET))
                        {
                            m_TargetStop = true;
                            if (OnTargetLanded!= null)
                            {
                                BasicBehaviour basicBehaviour = hit.collider.GetComponent<BasicBehaviour>();
                                OnTargetLanded.Invoke(basicBehaviour);
                            }
                        }
                    }
                }
            }
            if(!m_IsGrounded)
            {
                m_AnyStop = false;
                m_WaterStop = false;
                m_BullseyeStop = false;
                m_TargetStop = false;
            }
            Debug.DrawRay(transform.position, Vector3.down * m_DownCastDistance, Color.red);
            Debug.DrawRay(transform.position, -m_RigidBody.velocity, Color.cyan);
        }
    }
}
using UnityEngine;
using System.Collections;

namespace LilyPadsEndlessJumper.PullShootManager
{
    [RequireComponent(typeof(Rigidbody))]
    public class PullShooter : MonoBehaviour
    {
        //[SerializeField]
        //float m_AngleSpeed = 2.0f;
        [SerializeField]
        LayerMask m_PlayerLayerMask = default(LayerMask);
        [SerializeField]
        LayerMask m_GroundLayerMask = default(LayerMask);

        [SerializeField]
        float m_JumpPower = 220.0f;
        [SerializeField]
        float m_PushPower = 250.0f;
        [SerializeField, Range(0.001f, 1.0f)]
        float m_AimSmoothTime = 0.3f;

        [SerializeField, Range(0.0f, 10.0f)]
        float m_MaxPullDistance = 4.0f;

        [Header("Line Renderer Settings")]
        [SerializeField, Range(1.0f, 5.0f)]
        float m_ForwardStretch = 1.0f;
        [SerializeField, Range(0.1f, 5.0f)]
        float m_AimHeight = 0.75f;

        [SerializeField]
        Color m_Color1 = Color.yellow;
        [SerializeField]
        Color m_Color2 = Color.red;
        [SerializeField]
        Color m_Color3 = Color.green;

        bool m_CanFire = false;
        bool m_StartShoot = false;
        Vector3 m_AimDirection = Vector3.zero;
        float m_Distance = 0.0f;

        public bool canFire { get { return m_CanFire; } }
        LineRenderer m_LineRenderer = null;
        Rigidbody m_RigidBody = null;

        void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_LineRenderer = GetComponent<LineRenderer>();
            m_LineRenderer.SetVertexCount(3);
            m_LineRenderer.SetPosition(0, transform.position);
            m_LineRenderer.SetPosition(1, transform.position);
            m_LineRenderer.SetPosition(2, transform.position);
            m_LineRenderer.SetColors(Color.clear, Color.clear);
            m_LineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            m_LineRenderer.receiveShadows = true;
            m_LineRenderer.useWorldSpace = true;
            m_LineRenderer.enabled = false;
        }

        Vector3 m_LastPoint = Vector3.zero;
        Vector3 m_Velocity = Vector3.zero;

        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(TouchShooter.GetInputPosition());

            if (TouchShooter.GetInputDown())
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    m_LastPoint = hit.point;
                    bool colliders = Physics.CheckSphere(m_LastPoint, 0.5f, m_PlayerLayerMask);
                    if (colliders)
                    {
                        m_StartShoot = true;
                        m_LineRenderer.enabled = true;
                        m_LineRenderer.SetColors(Color.clear, Color.clear);
                        m_LineRenderer.SetWidth(0.3F, 0.1F);
                    }
                }
            }
            else if (TouchShooter.GetInputContinued())
            {
                if (m_StartShoot)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        m_LastPoint = Vector3.SmoothDamp(m_LastPoint, hit.point, ref m_Velocity, m_AimSmoothTime);
                        Vector3 dir = transform.position - m_LastPoint;
                        m_Distance = Vector3.Distance(m_LastPoint, transform.position);
                        //distance = Mathf.Clamp(distance, 0.0f, m_MaxDistance);
                        if (m_Distance <= m_MaxPullDistance)
                        {
                            m_LineRenderer.SetColors(m_Color3, m_Color1);
                            m_CanFire = true;
                        }
                        else
                        {
                            m_LineRenderer.SetColors(m_Color2, m_Color2);
                            m_CanFire = false;
                        }
                        m_AimDirection = transform.position - m_LastPoint;
                        Vector3 linePosition0 = m_LastPoint;
                        Vector3 linePosition1 = transform.position;
                        Vector3 linePosition2 = m_LastPoint + dir * m_Distance * m_ForwardStretch;

                        linePosition0.y = m_AimHeight;
                        linePosition1.y = m_AimHeight;
                        linePosition2.y = m_AimHeight;

                        m_LineRenderer.SetPosition(0, linePosition0);
                        m_LineRenderer.SetPosition(1, linePosition1);
                        m_LineRenderer.SetPosition(2, linePosition2);
                        Debug.DrawRay(hit.point, dir, Color.blue);
                    }
                }
            }
            else if (TouchShooter.GetInputUp())
            {
                if (m_StartShoot)
                {
                    m_StartShoot = false;
                    // Release and fire
                    m_LineRenderer.enabled = false;
                    m_LineRenderer.SetPosition(0, transform.position);
                    m_LineRenderer.SetPosition(1, transform.position);
                    m_LineRenderer.SetPosition(2, transform.position);
                    m_LineRenderer.SetColors(Color.clear, Color.clear);
                    if (m_CanFire)
                    {
                        m_RigidBody.AddForce(m_AimDirection * m_PushPower + Vector3.up * m_JumpPower);
                    }
                    m_CanFire = false;
                }
            }
        }
    }
}
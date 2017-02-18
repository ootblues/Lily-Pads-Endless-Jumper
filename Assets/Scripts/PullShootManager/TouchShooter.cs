using UnityEngine;
using System.Collections;

namespace LilyPadsEndlessJumper.PullShootManager
{
    [RequireComponent(typeof(Rigidbody))]
    public class TouchShooter : MonoBehaviour
    {
        public static Vector3 GetInputPosition()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return Input.mousePosition;
#else
            if(Input.touchCount == 1)
            {
                return Input.touches[0].position;
            }
            return Vector3.zero;
#endif
        }

        public static bool GetInputDown()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return Input.GetMouseButtonDown(0);
#else
            if (Input.touchCount == 1)
            {
                return Input.GetTouch(0).phase == TouchPhase.Began;
            }
            return false;
#endif
        }

        public static bool GetInputContinued()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return Input.GetMouseButton(0);
#else
            if (Input.touchCount == 1)
            {
                return Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary;
            }
            return false;
#endif
        }

        public static bool GetInputUp()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            return Input.GetMouseButtonUp(0);
#else
            if (Input.touchCount == 1)
            {
                return Input.GetTouch(0).phase == TouchPhase.Ended;
            }
            return false;
#endif
        }

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

        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(GetInputPosition());

            if (GetInputDown())
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 pos = hit.point;
                    bool colliders = Physics.CheckSphere(pos, 0.5f, m_PlayerLayerMask | m_GroundLayerMask);
                    if (colliders)
                    {
                        m_StartShoot = true;
                        m_LineRenderer.enabled = true;
                        m_LineRenderer.SetColors(Color.clear, Color.clear);
                        m_LineRenderer.SetWidth(0.3F, 0.1F);
                    }
                }
            }
            else if (GetInputContinued())
            {
                if (m_StartShoot)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 dir = transform.position - hit.point;
                        m_Distance = Vector3.Distance(hit.point, transform.position);
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
                        m_AimDirection = transform.position - hit.point;

                        Vector3 linePosition0 = hit.point;
                        Vector3 linePosition1 = transform.position;
                        Vector3 linePosition2 = hit.point + dir * m_Distance * m_ForwardStretch;

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
            else if (GetInputUp())
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
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PullShooter : MonoBehaviour
{
    //[SerializeField]
    //float m_AngleSpeed = 2.0f;
    [SerializeField]
    LayerMask playerLayerMask;
    [SerializeField]
    LayerMask groundLayerMask;

    [SerializeField]
    float m_UpPower = 20.0f;
    [SerializeField]
    float m_Power = 100.0f;
    [SerializeField, Range(1.0f, 5.0f)]
    float m_ForwardStretch = 1.0f;

    [SerializeField, Range(0.1f, 5.0f)]
    float m_AimHeight = 0.75f;

    [SerializeField]
    bool m_StartShoot = false;
    [SerializeField]
    Vector3 m_AimDirection = Vector3.zero;

    [SerializeField, Range(0.0f, 10.0f)]
    float m_MaxDistance = 4.0f;

    [SerializeField]
    Color m_Color1 = Color.yellow;
    [SerializeField]
    Color m_Color2 = Color.red;
    [SerializeField]
    Color m_Color3 = Color.green;
    [SerializeField]
    bool m_CanFire = false;

    public float distance = 0.0f;
    public bool canFire { get { return m_CanFire; } }
    LineRenderer m_LineRenderer = null;
    Rigidbody m_RigidBody = null;

    void Start ()
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

    void Update ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 pos = hit.point;
                bool colliders = Physics.CheckSphere(pos, 0.5f, playerLayerMask);
                if (colliders)
                {
                    m_StartShoot = true;
                    m_LineRenderer.enabled = true;
                    m_LineRenderer.SetColors(Color.clear, Color.clear);
                    m_LineRenderer.SetWidth(0.3F, 0.1F);
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if(m_StartShoot)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 pos = hit.point;
                    bool colliders = Physics.CheckSphere(pos, 0.5f, groundLayerMask);
                    if (colliders)
                    {
                        Vector3 dir = transform.position - pos;
                        distance = Vector3.Distance(pos, transform.position);
                        //distance = Mathf.Clamp(distance, 0.0f, m_MaxDistance);
                        if (distance <= m_MaxDistance)
                        {
                            m_LineRenderer.SetColors(m_Color3, m_Color1);
                            m_CanFire = true;
                        }
                        else
                        {
                            m_LineRenderer.SetColors(m_Color2, m_Color2);
                            m_CanFire = false;
                        }
                        m_AimDirection = transform.position - pos;

                        Vector3 linePosition0 = pos;
                        Vector3 linePosition1 = transform.position;
                        Vector3 linePosition2 = hit.point + dir * distance * m_ForwardStretch;

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
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Release and fire
            m_LineRenderer.enabled = false;
            m_LineRenderer.SetPosition(0, transform.position);
            m_LineRenderer.SetPosition(1, transform.position);
            m_LineRenderer.SetPosition(2, transform.position);
            m_LineRenderer.SetColors(Color.clear, Color.clear);
            if (m_CanFire)
            {
                m_RigidBody.AddForce(m_AimDirection * m_Power + Vector3.up * m_UpPower);
            }
            m_CanFire = false;
        }
    }
}

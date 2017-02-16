using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SlowVelocityStop : MonoBehaviour
{
    public static event System.Action OnGroundEntered;

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

    void Start ()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void Update ()
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
                    if(hit.collider.CompareTag(Tags.GROUND))
                    {
                        if (OnGroundEntered != null)
                        {
                            OnGroundEntered.Invoke();
                        }
                    }
                }
            }
        }
        Debug.DrawRay(transform.position, Vector3.down * m_DownCastDistance, Color.red);
        Debug.DrawRay(transform.position, -m_RigidBody.velocity, Color.cyan);
    }
}

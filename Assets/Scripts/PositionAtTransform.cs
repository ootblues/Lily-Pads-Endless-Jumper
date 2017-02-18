using UnityEngine;
using System.Collections;

namespace LilyPadsEndlessJumper
{
    public class PositionAtTransform : MonoBehaviour
    {
        [SerializeField]
        protected Transform m_AtTransform = null;
        [SerializeField]
        float m_SmoothTime = 5.0f;
        [SerializeField]
        protected Vector3 m_Offset = Vector3.zero;
        Vector3 m_Velocity = Vector3.zero;

        public virtual Vector3 targetPosition { get { return m_AtTransform.position + m_Offset; } }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_Velocity, m_SmoothTime * Time.deltaTime);
        }
    }
}
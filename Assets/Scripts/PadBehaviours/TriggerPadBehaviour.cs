using UnityEngine;
using System.Collections;

namespace LilyPadsEndlessJumper.PadBehaviours
{
    public class TriggerPadBehaviour : BasicBehaviour
    {
        //[SerializeField]
        SlowVelocityStop m_SlowVelocityStop = null;

        protected override void Start()
        {
            m_SlowVelocityStop = FindObjectOfType<SlowVelocityStop>();
            ResetPad();
        }

        public override void ResetPad()
        {
            base.ResetPad();
            m_CanUpdate = true;
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == m_SlowVelocityStop.gameObject)
            {
                if (!m_PadEntered)
                {
                    m_PadEntered = true;
                    InvokeOnPadEntered(this);
                }
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (other.gameObject == m_SlowVelocityStop.gameObject)
            {
                if (m_SlowVelocityStop.hasStopped)
                {
                    if (!m_PadStopped)
                    {
                        m_PadStopped = true;
                        m_CanUpdate = false;
                        InvokeOnPadStopped(this);
                    }
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.gameObject == m_SlowVelocityStop.gameObject)
            {
                if (!m_PadExited)
                {
                    m_PadExited = true;
                    InvokeOnPadExited(this);
                }
            }
        }
    }
}
using UnityEngine;
using System.Collections;

public class EnterPadTrigger : MonoBehaviour
{
    public static event System.Action<EnterPadTrigger> OnPadEntered;
    public static event System.Action<EnterPadTrigger> OnPadStopped;


    [SerializeField]
    GameObject m_PadTargetGO = null;
    [SerializeField]
    GameObject m_PadBullseyeGO = null;

    public GameObject padTargetGO { get { return m_PadTargetGO; } }
    public GameObject padBullseyeGO { get { return m_PadBullseyeGO; } }

    [SerializeField]
    bool m_PadEntered = false;
    [SerializeField]
    bool m_PadStopped = false;

    [SerializeField]
    SlowVelocityStop m_SlowVelocityStop = null;

    void Start()
    {
        m_SlowVelocityStop = FindObjectOfType<SlowVelocityStop>();
    }

    public void ResetPad()
    {
        m_PadEntered = false;
        m_PadStopped = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!m_PadEntered)
        {
            m_PadEntered = true;
            if (OnPadEntered != null)
            {
                OnPadEntered.Invoke(this);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(!m_PadStopped)
        {
            if (other.gameObject == m_SlowVelocityStop.gameObject)
            {
                if (m_SlowVelocityStop.hasStopped)
                {
                    m_PadStopped = true;
                    if (OnPadStopped != null)
                    {
                        OnPadStopped.Invoke(this);
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {

    }

    //void OnCollisionEnter(Collision other)
    //{
    //    if (!m_PadEntered)
    //    {
    //        m_PadEntered = true;
    //        if (OnPadEntered != null)
    //        {
    //            OnPadEntered.Invoke(this);
    //        }
    //    }
    //}

    //void OnCollisionStay(Collision other)
    //{
    //    if (!m_PadStopped)
    //    {
    //        if (other.gameObject == m_SlowVelocityStop.gameObject)
    //        {
    //            if (m_SlowVelocityStop.hasStopped)
    //            {
    //                m_PadStopped = true;
    //                if (OnPadStopped != null)
    //                {
    //                    OnPadStopped.Invoke(this);
    //                }
    //            }
    //        }
    //    }
    //}

    //void OnCollisionExit(Collision other)
    //{

    //}
}

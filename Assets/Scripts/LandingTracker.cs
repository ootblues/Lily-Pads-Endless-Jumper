using UnityEngine;
using System.Collections;
using System;

public class LandingTracker : MonoBehaviour
{
    [SerializeField]
    GameObject m_PlayerGO = null;
    [SerializeField]
    TargetGenerator m_TargetGenerator = null;

    [SerializeField]
    int m_TargetLandBonusCount = 5;
    [SerializeField]
    int m_TargetLandCount = 0;
    [SerializeField]
    int m_BullseyeLandBonusCount = 3;
    [SerializeField]
    int m_BullseyeLandCount = 0;
    [SerializeField]
    int m_ContinousLandBonusCount = 10;
    [SerializeField]
    int m_ContinousLandCount = 0;

    Vector3 m_LastPlayerPosition = Vector3.zero;
    void OnEnable()
    {
        PadStopLocator.OnWhichPad -= OnWhichPadStopped;
        PadStopLocator.OnWhichPad += OnWhichPadStopped;
        SlowVelocityStop.OnGroundEntered -= OnGroundEntered;
        SlowVelocityStop.OnGroundEntered += OnGroundEntered;
        m_LastPlayerPosition = m_PlayerGO.transform.position;
    }

    void OnDisable()
    {
        PadStopLocator.OnWhichPad -= OnWhichPadStopped;
        SlowVelocityStop.OnGroundEntered -= OnGroundEntered;
    }

    void OnGroundEntered()
    {

        m_TargetLandCount = 0;
        m_BullseyeLandCount = 0;
        m_ContinousLandCount = 0;
        m_TargetGenerator.baseScale = 1.0f;

        ResetLastPadScale();
        m_PlayerGO.GetComponent<Rigidbody>().velocity = Vector3.zero;
        m_PlayerGO.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        m_PlayerGO.transform.position = m_LastPlayerPosition + Vector3.up;
    }

    void OnWhichPadStopped(PadStop padStop, int hitCount)
    {
        m_LastPlayerPosition = m_PlayerGO.transform.position;
        switch (padStop)
        {
            case PadStop.None:
                m_TargetLandCount = 0;
                m_BullseyeLandCount = 0;
                m_ContinousLandCount = 0;
                m_TargetGenerator.baseScale = 1.0f;
                ResetLastPadScale();
                break;
            case PadStop.Target:
                m_TargetLandCount++;
                m_BullseyeLandCount = 0;
                m_ContinousLandCount++;
                break;
            case PadStop.Bullseye:
                m_BullseyeLandCount++;
                m_TargetLandCount++;
                m_ContinousLandCount++;
                break;
            default:
                break;
        }

        if (m_TargetLandCount > m_TargetLandBonusCount)
        {
            m_TargetLandCount = 0;
            m_TargetGenerator.baseScale -= 0.1f;
        }
        if (m_BullseyeLandCount > m_BullseyeLandBonusCount)
        {
            m_BullseyeLandCount = 0;
            m_TargetGenerator.baseScale -= 0.15f;
        }
        if (m_ContinousLandCount > m_ContinousLandBonusCount)
        {
            m_ContinousLandCount = 0;
            m_TargetGenerator.baseScale -= 0.05f;
        }
    }

    void ResetLastPadScale()
    {
        if (m_TargetGenerator.nextPadTrigger)
        {
            m_TargetGenerator.nextPadTrigger.transform.localScale = m_TargetGenerator.prefabScale * m_TargetGenerator.baseScale;
        }
    }
}

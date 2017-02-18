using UnityEngine;
using System.Collections;
using System.Linq;
using LilyPadsEndlessJumper.PadBehaviours.BehaviourModifiers;
using LilyPadsEndlessJumper.PadBehaviours;
using System.Collections.Generic;

namespace LilyPadsEndlessJumper
{
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

        [SerializeField]
        BasicBehaviourModifier m_DefaultBehaviourModifier = null;
        [SerializeField]
        List<BasicBehaviourModifier> m_BehaviourModifiers = null;

        [SerializeField]
        int m_NextModifierCount = 0;

        Vector3 m_LastPlayerPosition = Vector3.zero;
        void OnEnable()
        {
            TargetGenerator.OnNextPadGenerated -= OnNextPadGenerated;
            TargetGenerator.OnNextPadGenerated += OnNextPadGenerated;
            PadStopLocator.OnWhichPad -= OnWhichPadStopped;
            PadStopLocator.OnWhichPad += OnWhichPadStopped;
            SlowVelocityStop.OnWaterLanded -= OnWaterLanded;
            SlowVelocityStop.OnWaterLanded += OnWaterLanded;
            m_LastPlayerPosition = m_PlayerGO.transform.position;
        }

        void OnDisable()
        {
            TargetGenerator.OnNextPadGenerated -= OnNextPadGenerated;
            PadStopLocator.OnWhichPad -= OnWhichPadStopped;
            SlowVelocityStop.OnWaterLanded -= OnWaterLanded;
        }

        void OnWaterLanded(BasicBehaviour nextBehaviour)
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

            //if (m_TargetLandCount > m_TargetLandBonusCount)
            //{
            //    m_TargetLandCount = 0;
            //    m_TargetGenerator.baseScale -= 0.1f;
            //}
            //if (m_BullseyeLandCount > m_BullseyeLandBonusCount)
            //{
            //    m_BullseyeLandCount = 0;
            //    m_TargetGenerator.baseScale -= 0.15f;
            //}
            //if (m_ContinousLandCount > m_ContinousLandBonusCount)
            //{
            //    m_ContinousLandCount = 0;
            //    m_TargetGenerator.baseScale -= 0.05f;
            //}
        }
        
        void OnNextPadGenerated(BasicBehaviour nextPadBehaviour)
        {
            if (m_BullseyeLandCount > m_BullseyeLandBonusCount)
            {
                if (m_BullseyeLandCount == m_BullseyeLandBonusCount + 1)
                {
                    m_TargetGenerator.baseScale -= 0.15f;
                }
                else
                {
                    m_TargetGenerator.baseScale -= 0.01f;
                }
                //m_BullseyeLandCount = 0;

                AddBehaviourModifierToPad(nextPadBehaviour);
            }
            else if (m_TargetLandCount > m_TargetLandBonusCount)
            {
                m_TargetLandCount = 0;
                m_TargetGenerator.baseScale -= 0.1f;
            }
            else if (m_ContinousLandCount > m_ContinousLandBonusCount)
            {
                m_ContinousLandCount = 0;
                m_TargetGenerator.baseScale -= 0.05f;
            }
        }

        void AddBehaviourModifierToPad(BasicBehaviour nextPadBehaviour)
        {
            if (nextPadBehaviour)
            {
                m_NextModifierCount++;
                m_NextModifierCount = Mathf.Clamp(m_NextModifierCount, 0, m_BehaviourModifiers.Count);
                for (int i = 0; i < m_NextModifierCount; i++)
                {
                    BasicBehaviourModifier behaviourModifier = m_BehaviourModifiers[i];
                    Debug.Log("Adding Modifier of type: " + behaviourModifier.GetType());
                    nextPadBehaviour.behaviourModifiers.Add(behaviourModifier);
                }
            }
        }

        void ResetLastPadScale()
        {
            if (m_TargetGenerator.nextPadBehaviour)
            {
                //m_TargetGenerator.nextPadBehaviour.transform.localScale = m_TargetGenerator.prefabScale * m_TargetGenerator.baseScale;

                m_TargetGenerator.UseScaleBehaviourModifier(m_TargetGenerator.nextPadBehaviour);
            }
            //if (m_TargetGenerator.nextPadBehaviour)
            //{
            //    m_TargetGenerator.nextPadBehaviour.behaviourModifier = m_BasicBehaviourModifier;
            //}
        }

    }
}
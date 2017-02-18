using UnityEngine;
using System.Collections;
using System;
using LilyPadsEndlessJumper.PadBehaviours;

namespace LilyPadsEndlessJumper
{
    public enum PadStop
    {
        None,
        Target,
        Bullseye
    }
    public class PadStopLocator : PadInteraction
    {
        static Vector3[] offsets = new Vector3[]
        {
        Vector3.zero,
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right,
        };

        [SerializeField, Range(0.01f, 1.0f)]
        float m_RayCastScaleRatio = 0.5f;
        [SerializeField]
        int m_RayCastCount = 5;

        [SerializeField]
        TargetGenerator m_TargetGenerator = null;

        [SerializeField]
        TriggerPadBehaviour m_NextEnterPadTrigger = null;

        public static event System.Action<PadStop, int> OnWhichPad;

        void Update()
        {
            m_NextEnterPadTrigger = m_TargetGenerator.nextPadBehaviour;
        }

        void OnDrawGizmos()
        {
            float rayCastScale = m_NextEnterPadTrigger ? m_NextEnterPadTrigger.padTargetGO.transform.localScale.x * m_RayCastScaleRatio : 1.0f;
            for (int i = 0; i < m_RayCastCount; i++)
            {
                Vector3 castPos = transform.position + offsets[i] * rayCastScale;
                Debug.DrawRay(castPos, Vector3.down);
            }
        }

        protected override void OnPadStopped(BasicBehaviour basicBehaviour)
        {
            //Debug.Log("Stopped");
            int targetHitCount = 0;
            int bullseyeHitCount = 0;

            GetAllHitTargets(ref targetHitCount, ref bullseyeHitCount);

            if (bullseyeHitCount > 0)
            {
                if (OnWhichPad != null)
                {
                    OnWhichPad.Invoke(PadStop.Bullseye, bullseyeHitCount);
                }
            }
            else if (targetHitCount > 0)
            {
                if (OnWhichPad != null)
                {
                    OnWhichPad.Invoke(PadStop.Target, 1);
                }
            }
            else
            {
                if (OnWhichPad != null)
                {
                    OnWhichPad.Invoke(PadStop.None, 0);
                }
            }
        }

        public void GetAllHitTargets(ref int targetHitCount, ref int bullseyeHitCount)
        {
            float rayCastScale = m_NextEnterPadTrigger ? m_NextEnterPadTrigger.padTargetGO.transform.localScale.x * m_RayCastScaleRatio : 1.0f;
            for (int i = 0; i < m_RayCastCount; i++)
            {
                Vector3 castPos = transform.position + offsets[i] * rayCastScale;
                RaycastHit hit;
                if (Physics.Raycast(castPos, Vector3.down, out hit))
                {
                    if (hit.collider.CompareTag(Tags.TARGET))
                    {
                        targetHitCount += 1;
                    }
                    else if (hit.collider.CompareTag(Tags.BULLSEYE_TARGET))
                    {
                        if (OnWhichPad != null)
                        {
                            bullseyeHitCount += 1;
                        }
                    }
                }
            }
        }

        protected override void OnWhichPadStopped(PadStop padStop, int hitCount)
        {
            //Debug.LogFormat("padStop: {0} hitCount: {1}", padStop, hitCount);
        }
    }
}
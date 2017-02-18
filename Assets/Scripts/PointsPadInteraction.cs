using LilyPadsEndlessJumper.PadBehaviours;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LilyPadsEndlessJumper
{
    public class PointsPadInteraction : PadInteraction
    {
        [SerializeField]
        Text m_PointsText = null;

        [SerializeField]
        Text m_BonusPointsText = null;

        [SerializeField]
        int m_BullseyeBonus = 2;
        [SerializeField]
        float m_BonusShowTime = 1.0f;

        [SerializeField]
        int m_Points = 0;

        void Start()
        {
            m_BonusPointsText.enabled = false;
        }

        protected override void OnWhichPadStopped(PadStop padStop, int hitCount)
        {
            switch (padStop)
            {
                case PadStop.None:
                    break;
                case PadStop.Target:
                    m_Points++;
                    break;
                case PadStop.Bullseye:
                    m_Points += m_BullseyeBonus * hitCount;
                    StartCoroutine(ShowBonusText(hitCount));
                    break;
                default:
                    break;
            }
            m_PointsText.text = string.Format("Points: {0}", m_Points);
        }

        IEnumerator ShowBonusText(int hitCount)
        {
            yield return null;
            m_BonusPointsText.enabled = true;
            m_BonusPointsText.text = string.Format("Bonus +{0}", m_BullseyeBonus * hitCount);
            yield return new WaitForSeconds(m_BonusShowTime);
            m_BonusPointsText.enabled = false;
        }
    }
}
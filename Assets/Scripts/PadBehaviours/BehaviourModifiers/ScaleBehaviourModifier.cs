using UnityEngine;
using System.Collections;
using System;

namespace LilyPadsEndlessJumper.PadBehaviours.BehaviourModifiers
{
    [CreateAssetMenu]
    public class ScaleBehaviourModifier : BasicBehaviourModifier
    {
        [SerializeField]
        float m_MinimumScale = 2.0f;
        [SerializeField]
        float m_MaximumScale = 4.0f;

        public float MinimumScale { get { return m_MinimumScale; } }
        public float MaximumScale { get { return m_MaximumScale; } }
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace LilyPadsEndlessJumper
{
    public class DebugPositionAtTransform : PositionAtTransform
    {
        [SerializeField]
        Text m_PadIdText = null;

        [SerializeField]
        Text m_PadScaleText = null;

        public virtual Vector3 targetPosition { get { return m_AtTransform.position + m_Offset; } }

        TargetGenerator m_TargetGenerator = null;

        protected override void Start()
        {
            m_TargetGenerator = FindObjectOfType<TargetGenerator>();
        }

        protected override void Update()
        {
            int padIndex = m_TargetGenerator.nextPadBehaviour ? m_TargetGenerator.nextPadBehaviour.worldIndex : -1;
            m_PadIdText.text = string.Format("Pad: {0}", padIndex);

            float padScale = m_TargetGenerator.nextPadBehaviour ? m_TargetGenerator.nextPadBehaviour.transform.localScale.x : -1.0f;
            m_PadScaleText.text = string.Format("Scale: {0}", padScale);

            m_AtTransform = m_TargetGenerator.nextPadBehaviour ? m_TargetGenerator.nextPadBehaviour.transform : m_AtTransform;
            base.Update();
        }
    }
}
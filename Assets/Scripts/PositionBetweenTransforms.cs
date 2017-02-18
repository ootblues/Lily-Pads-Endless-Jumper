using UnityEngine;
using System.Collections;

namespace LilyPadsEndlessJumper
{
    public class PositionBetweenTransforms : PositionAtTransform
    {
        public Transform firstTransform = null;
        public Transform secondTransform = null;

        public override Vector3 targetPosition
        {
            get
            {
                return Vector3.Lerp(firstTransform.position, secondTransform.position, 0.5f) + m_Offset;
            }
        }

    }
}
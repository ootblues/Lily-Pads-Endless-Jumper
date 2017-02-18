using UnityEngine;
using System.Collections;

namespace LilyPadsEndlessJumper.PadBehaviours
{
    public class BullseyeTriggerPadBehaviour : TriggerPadBehaviour
    {
        [SerializeField]
        GameObject m_PadBullseyeGO = null;

        public GameObject padBullseyeGO { get { return m_PadBullseyeGO; } }
    }
}
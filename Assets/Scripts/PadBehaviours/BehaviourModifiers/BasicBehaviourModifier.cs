using UnityEngine;
using System.Collections;
using System;

namespace LilyPadsEndlessJumper.PadBehaviours.BehaviourModifiers
{
    public abstract class BasicBehaviourModifier : ScriptableObject
    {
        public bool stopUpdateOnLanding = false;

        public virtual void DoUpdate(BasicBehaviour basicBehaviour)
        {

        }

        public virtual void ResetPad()
        {
        }
    }
}
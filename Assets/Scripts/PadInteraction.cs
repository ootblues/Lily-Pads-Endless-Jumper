using LilyPadsEndlessJumper.PadBehaviours;
using LilyPadsEndlessJumper.PadBehaviours.BehaviourModifiers;
using System;
using UnityEngine;

namespace LilyPadsEndlessJumper
{
    public abstract class PadInteraction : MonoBehaviour
    {
        void OnEnable()
        {
            BasicBehaviour.OnPadEntered -= OnPadEntered;
            BasicBehaviour.OnPadEntered += OnPadEntered;
            BasicBehaviour.OnPadStopped -= OnPadStopped;
            BasicBehaviour.OnPadStopped += OnPadStopped;
            BasicBehaviour.OnPadExited -= OnPadExited;
            BasicBehaviour.OnPadExited += OnPadExited;
            PadStopLocator.OnWhichPad -= OnWhichPadStopped;
            PadStopLocator.OnWhichPad += OnWhichPadStopped;
        }

        void OnDisable()
        {
            BasicBehaviour.OnPadEntered -= OnPadEntered;
            BasicBehaviour.OnPadStopped -= OnPadStopped;
            BasicBehaviour.OnPadExited -= OnPadExited;
            PadStopLocator.OnWhichPad -= OnWhichPadStopped;
        }

        protected virtual void OnWhichPadStopped(PadStop padStop, int hitCount)
        {

        }

        protected virtual void OnPadEntered(BasicBehaviour basicBehaviour)
        {

        }

        protected virtual void OnPadStopped(BasicBehaviour basicBehaviour)
        {

        }

        protected virtual void OnPadExited(BasicBehaviour basicBehaviour)
        {

        }
    }
}
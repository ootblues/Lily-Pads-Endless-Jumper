using System;
using UnityEngine;

public abstract class PadInteraction : MonoBehaviour
{
    void OnEnable()
    {
        EnterPadTrigger.OnPadEntered -= OnPadEntered;
        EnterPadTrigger.OnPadEntered += OnPadEntered;
        EnterPadTrigger.OnPadStopped -= OnPadStopped;
        EnterPadTrigger.OnPadStopped += OnPadStopped;
        PadStopLocator.OnWhichPad -= OnWhichPadStopped;
        PadStopLocator.OnWhichPad += OnWhichPadStopped;
    }

    void OnDisable()
    {
        EnterPadTrigger.OnPadEntered -= OnPadEntered;
        EnterPadTrigger.OnPadStopped -= OnPadStopped;
        PadStopLocator.OnWhichPad -= OnWhichPadStopped;
    }

    protected abstract void OnWhichPadStopped(PadStop padStop, int hitCount);

    protected abstract void OnPadEntered(EnterPadTrigger enterPadTrigger);

    protected abstract void OnPadStopped(EnterPadTrigger enterPadTrigger);
}

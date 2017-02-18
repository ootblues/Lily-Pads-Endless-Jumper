using UnityEngine;
using System.Collections;
using System;

namespace LilyPadsEndlessJumper.PadBehaviours.BehaviourModifiers
{
    [CreateAssetMenu]
    public class MovementBehaviourModifier : BasicBehaviourModifier
    {
        public Vector3 direction = Vector3.zero;
        public float speed = 1.0f;
        public float maximumDifference = 0.5f;

        public override void DoUpdate(BasicBehaviour basicBehaviour)
        {
            basicBehaviour.time += Time.deltaTime;
            basicBehaviour.currentSpeed = Mathf.PingPong(basicBehaviour.time * speed, maximumDifference) - (maximumDifference * 0.5f);

            basicBehaviour.transform.Translate(direction * basicBehaviour.currentSpeed * Time.deltaTime);
        }
    }
}
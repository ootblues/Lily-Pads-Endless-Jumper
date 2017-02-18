using UnityEngine;
using System.Collections;
using LilyPadsEndlessJumper.PadBehaviours.BehaviourModifiers;
using System.Collections.Generic;

namespace LilyPadsEndlessJumper.PadBehaviours
{
    public abstract class BasicBehaviour : MonoBehaviour
    {
        public static event System.Action<BasicBehaviour> OnPadEntered;
        public static event System.Action<BasicBehaviour> OnPadStopped;
        public static event System.Action<BasicBehaviour> OnPadExited;

        public virtual void InvokeOnPadEntered(BasicBehaviour basicBehaviour)
        {
            if (OnPadEntered != null)
            {
                OnPadEntered.Invoke(basicBehaviour);
            }
        }

        public virtual void InvokeOnPadStopped(BasicBehaviour basicBehaviour)
        {
            if (OnPadStopped != null)
            {
                OnPadStopped.Invoke(basicBehaviour);
            }
        }

        public virtual void InvokeOnPadExited(BasicBehaviour basicBehaviour)
        {
            if (OnPadExited != null)
            {
                OnPadExited.Invoke(basicBehaviour);
            }
        }

        [SerializeField]
        protected List<BasicBehaviourModifier> m_BehaviourModifiers = null;

        //[SerializeField]
        protected int m_WorldIndex = -1;
        //[SerializeField]
        protected bool m_PadEntered = false;
        //[SerializeField]
        protected bool m_PadStopped = false;
        //[SerializeField]
        protected bool m_PadExited = false;

        [SerializeField]
        GameObject m_PadTargetGO = null;

        protected float m_Time = 0.0f;
        protected float m_CurrentSpeed = 0.0f;

        protected bool m_CanUpdate = false;

        public GameObject padTargetGO { get { return m_PadTargetGO; } }

        public int worldIndex { get { return m_WorldIndex; } set { m_WorldIndex = value; } }
        public float time { get { return m_Time; } set { m_Time = value; } }
        public float currentSpeed { get { return m_CurrentSpeed; } set { m_CurrentSpeed = value; } }
        public bool canUpdate { get { return m_CanUpdate; } protected set { m_CanUpdate = value; } }
        public bool padEntered { get { return m_PadEntered; } protected set { m_PadEntered = value; } }
        public bool padStopped { get { return m_PadStopped; } protected set { m_PadStopped = value; } }
        public bool padExited { get { return m_PadExited; } protected set { m_PadExited = value; } }

        public List<BasicBehaviourModifier> behaviourModifiers { get { return m_BehaviourModifiers; } set { m_BehaviourModifiers = value; } }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        public virtual void ResetPad()
        {
            m_PadEntered = false;
            m_PadStopped = false;
            m_PadExited = false;
            m_CanUpdate = false;
            for (int i = 0; i < m_BehaviourModifiers.Count; i++)
            {
                m_BehaviourModifiers[i].ResetPad();
            }
        }

        protected virtual void Update()
        {
            for (int i = 0; i < m_BehaviourModifiers.Count; i++)
            {
                if (m_CanUpdate || !m_BehaviourModifiers[i].stopUpdateOnLanding)
                {
                    m_BehaviourModifiers[i].DoUpdate(this);
                }
            }
        }
    }
}
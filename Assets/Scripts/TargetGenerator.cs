using UnityEngine;
using System.Collections;
using LilyPadsEndlessJumper.PadBehaviours;
using LilyPadsEndlessJumper.PadBehaviours.BehaviourModifiers;

namespace LilyPadsEndlessJumper
{
    public class TargetGenerator : MonoBehaviour
    {
        public static event System.Action<TriggerPadBehaviour> OnNextPadGenerated;

        [SerializeField]
        GameObject m_StartPadGO = null;

        [SerializeField]
        GameObject m_PadPrefab = null;

        [SerializeField]
        int m_Seed = 1;

        [SerializeField]
        float m_BaseHeight = 0.05f;

        [SerializeField]
        int m_MinimumDistance = 3;
        [SerializeField]
        int m_MaximumDistance = 7;

        [SerializeField]
        int m_LevelWidth = 10;

        [SerializeField]
        PositionBetweenTransforms m_PositionBetweenTransforms = null;

        Vector3 m_LastPosition = Vector3.zero;

        public TriggerPadBehaviour nextPadBehaviour { get; private set; }
        public TriggerPadBehaviour currentPadBehaviour { get; private set; }
        public Vector3 prefabScale { get { return m_PadPrefab.transform.localScale; } }

        System.Random m_Rnd = null;
        public float baseScale = 1.0f;
        [SerializeField]
        int m_AmountGenerated = 0;

        void Start()
        {
            m_Rnd = new System.Random(m_Seed);
            //StartCoroutine(TestPlacement());
            m_LastPosition = m_StartPadGO.transform.position;
        }

        void OnEnable()
        {
            BasicBehaviour.OnPadStopped -= EnterPadTrigger_OnPadStopped;
            BasicBehaviour.OnPadStopped += EnterPadTrigger_OnPadStopped;
        }

        void OnDisable()
        {
            BasicBehaviour.OnPadStopped -= EnterPadTrigger_OnPadStopped;
        }

        void EnterPadTrigger_OnPadStopped(BasicBehaviour basicBehaviour)
        {
            StartCoroutine(NextPlacement(basicBehaviour));
        }

        IEnumerator NextPlacement(BasicBehaviour basicBehaviour)
        {
            yield return null;
            m_LastPosition = basicBehaviour.transform.position;
            Vector3 newPosition = m_LastPosition;
            bool placing = true;
            m_PositionBetweenTransforms.firstTransform = m_PositionBetweenTransforms.secondTransform;
            while (placing)
            {
                yield return null;

                int rx = m_Rnd.Next(-m_MaximumDistance, m_MaximumDistance) / 2;
                int rz = m_Rnd.Next(m_MinimumDistance, m_MaximumDistance);
                //Debug.LogFormat("rx:{0} rz:{1}", rx, rz);

                float px = m_LastPosition.x + rx;
                float pz = m_LastPosition.z + rz;
                float py = m_BaseHeight;

                if (px > -m_LevelWidth && px < m_LevelWidth)
                {
                    placing = false;
                    newPosition = new Vector3(px, py, pz);
                    nextPadBehaviour = Instantiate(m_PadPrefab).GetComponent<TriggerPadBehaviour>();
                    UseScaleBehaviourModifier(nextPadBehaviour);
                    nextPadBehaviour.transform.position = newPosition;
                    nextPadBehaviour.worldIndex = m_AmountGenerated;
                    m_PositionBetweenTransforms.secondTransform = nextPadBehaviour.transform;
                    currentPadBehaviour = m_PositionBetweenTransforms.firstTransform.GetComponent<TriggerPadBehaviour>();
                    if (OnNextPadGenerated != null)
                    {
                        OnNextPadGenerated.Invoke(nextPadBehaviour);
                    }
                    m_AmountGenerated++;
                }
            }

            m_LastPosition = newPosition;
        }

        public void UseScaleBehaviourModifier(TriggerPadBehaviour triggerPadBehaviour)
        {
            ScaleBehaviourModifier scaleBehaviourModifier = triggerPadBehaviour.behaviourModifiers.Find(x => x.GetType() == typeof(ScaleBehaviourModifier)) as ScaleBehaviourModifier;

            if (scaleBehaviourModifier)
            {
                Vector3 newScale = prefabScale * baseScale;
                newScale.x = Mathf.Clamp(newScale.x, scaleBehaviourModifier.MinimumScale, scaleBehaviourModifier.MaximumScale);
                newScale.z = Mathf.Clamp(newScale.z, scaleBehaviourModifier.MinimumScale, scaleBehaviourModifier.MaximumScale);
                triggerPadBehaviour.transform.localScale = newScale;
            }
        }
    }
}
using UnityEngine;
using System.Collections;
using LilyPadsEndlessJumper.PadBehaviours;
using System;

namespace LilyPadsEndlessJumper
{
    [RequireComponent(typeof(AudioSource), typeof(PadStopLocator))]
    public class PlayerActionSFX : MonoBehaviour
    {
        [SerializeField]
        AudioClip[] m_TargetLandClips = null;
        [SerializeField]
        AudioClip[] m_BullseyeLandClips = null;
        [SerializeField]
        AudioClip[] m_MissClips = null;
        AudioSource m_AudioSource = null;
        PadStopLocator m_PadStopLocator = null;

        void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
            m_PadStopLocator = GetComponent<PadStopLocator>();
        }

        void OnEnable()
        {
            SlowVelocityStop.OnLanded -= OnLanded;
            SlowVelocityStop.OnLanded += OnLanded;
            SlowVelocityStop.OnWaterLanded -= OnWaterLanded;
            SlowVelocityStop.OnWaterLanded += OnWaterLanded;
            SlowVelocityStop.OnTargetLanded -= OnTargetLanded;
            SlowVelocityStop.OnTargetLanded += OnTargetLanded;
            SlowVelocityStop.OnBullseyeLanded -= OnBullseyeLanded;
            SlowVelocityStop.OnBullseyeLanded += OnBullseyeLanded;
        }

        void OnDisable()
        {
            SlowVelocityStop.OnLanded -= OnLanded;
            SlowVelocityStop.OnWaterLanded -= OnWaterLanded;
            SlowVelocityStop.OnTargetLanded -= OnTargetLanded;
            SlowVelocityStop.OnBullseyeLanded -= OnBullseyeLanded;
        }

        void OnLanded(BasicBehaviour basicBehaviour)
        {
            if(basicBehaviour)
            {
                /*if(!basicBehaviour.CompareTag(Tags.GROUND))
                {
                    Vector3 distPos = transform.position;
                    distPos.y -= transform.localScale.y * 0.5f;
                    float dist = Vector3.Distance(distPos, basicBehaviour.transform.position);
                    float padDiameter = basicBehaviour.transform.localScale.x;
                    float padRadius = padDiameter * 0.5f;
                    float ratio = dist / padRadius;
                    Debug.LogFormat("How Close: ratio:{0} radius: {1} dist: {2}", ratio, padRadius, dist);
                }*/
            }
        }

        void OnTargetLanded(BasicBehaviour basicBehaviour)
        {
            AudioClip targetClip = m_TargetLandClips[UnityEngine.Random.Range(0, m_TargetLandClips.Length)];
            m_AudioSource.PlayOneShot(targetClip);
        }

        void OnBullseyeLanded(BasicBehaviour basicBehaviour)
        {
            int targetHitCount = 0;
            int bullseyeHitCount = 0;
            m_PadStopLocator.GetAllHitTargets(ref targetHitCount, ref bullseyeHitCount);

            //Debug.LogFormat("bullseyeHitCount: {0}", bullseyeHitCount);
            if (bullseyeHitCount == 5)
            {
                AudioClip bullseyeClip = m_BullseyeLandClips[0];
                m_AudioSource.PlayOneShot(bullseyeClip);
            }
            else if (bullseyeHitCount == 4)
            {
                AudioClip bullseyeClip = m_BullseyeLandClips[UnityEngine.Random.Range(1, 3)];
                m_AudioSource.PlayOneShot(bullseyeClip);
            }
            else
            {
                AudioClip bullseyeClip = m_BullseyeLandClips[UnityEngine.Random.Range(3, m_BullseyeLandClips.Length)];
                m_AudioSource.PlayOneShot(bullseyeClip);
            }
        }

        void OnWaterLanded(BasicBehaviour basicBehaviour)
        {
            AudioClip clip = m_MissClips[UnityEngine.Random.Range(0, m_MissClips.Length)];
            m_AudioSource.PlayOneShot(clip);
        }
    }
}
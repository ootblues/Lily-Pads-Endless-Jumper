using System.Diagnostics;
using System.Timers;
using UnityEngine;
using ValueTypes;

namespace PullShootManager
{

    [System.Serializable]
    public struct PullInfo
    {
        public const float LOWEST_MAX_DISTANCE = 5.0f;
        public const float HIGHEST_MAX_DISTANCE = 100.0f;

        public delegate void PullRangeChanged(PullInfo pullInfo);
        public static event PullRangeChanged OnPullRangeChanged;

        public delegate void PullFired(PullInfo pullInfo);
        public static event PullFired OnPullFired;

        [SerializeField]
        LayerMask layerMask;
        [SerializeField]
        RangeInfo m_RangeInfo;
        bool m_Started;
        bool m_Fired;

        long m_RatioPercent;
        Stopwatch m_Timer;

        public long percentage { get { return m_RatioPercent; } }

        public long time { get { return m_Timer.ElapsedMilliseconds; } }

        public RangeInfo rangeInfo { get { return m_RangeInfo; } }

        public bool started
        {
            get { return m_Started; }
            set
            {
                if(value)
                {
                    //Start timer
                    if(m_Timer == null)
                    {
                        m_Timer = new Stopwatch();
                    }
                    m_Timer.Reset();
                    m_Timer.Start();
                }
                m_Started = value;
            }
        }

        public bool fired
        {
            get { return m_Fired; }
            set
            {
                if(value)
                {
                    //Stop timer
                    m_Timer.Stop();
                    m_RatioPercent = (long)(m_RangeInfo.ratio * 100.0f);
                }
                m_Fired = value;
                if(OnPullFired != null)
                {
                    OnPullFired(this);
                }
            }
        }

        public bool canFire { get { return m_RangeInfo.value > 0 && m_RangeInfo.value > m_RangeInfo.min && m_RangeInfo.value <= m_RangeInfo.max; } }

        internal void UpdateValues(float minDistance, float maxDistance, float distance)
        {
            this.m_RangeInfo.min = Mathf.Clamp(minDistance, 0, PullInfo.HIGHEST_MAX_DISTANCE);
            this.m_RangeInfo.max = Mathf.Clamp(maxDistance, PullInfo.LOWEST_MAX_DISTANCE, PullInfo.HIGHEST_MAX_DISTANCE);
            this.m_RangeInfo.value = distance;
            if(OnPullRangeChanged != null)
            {
                OnPullRangeChanged(this);
            }
        }

        internal int GetLayerMask()
        {
            return layerMask;
        }

        internal RangeInfo GetRangeInfo()
        {
            return m_RangeInfo;
        }

        internal void Stop()
        {
            started = false;
            fired = false;
            UpdateValues(0.0f, PullInfo.LOWEST_MAX_DISTANCE, 0.0f);
        }
    }

}
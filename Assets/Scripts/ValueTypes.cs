using System.Runtime.Serialization;
using UnityEngine;

namespace LilyPadsEndlessJumper
{
    namespace ValueTypes
    {
        [System.Serializable]
        public struct LimitInfo
        {
            [SerializeField]
            public float min;
            [SerializeField]
            public float max;

            public LimitInfo(float min, float max)
            {
                this.min = min;
                this.max = max;
            }

            public LimitInfo(float max)
            {
                this.min = 0;
                this.max = max;
            }
        }

        [System.Serializable]
        public struct RangeInfo
        {
            [SerializeField]
            LimitInfo limit;
            [SerializeField]
            public float value;

            public float min { get { return limit.min; } set { limit.min = value; } }
            public float max { get { return limit.max; } set { limit.max = value; } }
            public float ratio { get { return this.value / limit.max; } }

            public RangeInfo(float value, float min, float max)
            {
                limit = new LimitInfo(min, max);
                this.value = value;
            }

            public RangeInfo(float min, float max)
            {
                limit = new LimitInfo(min, max);
                this.value = 0;
            }

        }

        public struct TimerInfo
        {
            public static string GetFormatted(float time)
            {
                System.TimeSpan t = GetTimeSpan(time);
                return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            }

            public static System.TimeSpan GetTimeSpan(float time)
            {
                return System.TimeSpan.FromSeconds(time);
            }

            float duration;
            float lastTime;
            float time;
            bool started;
            bool stopped;

            public void ReduceBy(float time)
            {
                CheckArgument(time, "ReduceBy");
                this.time -= time;
            }

            public void IncreaseBy(float time)
            {
                CheckArgument(time, "IncreaseBy");
                this.time += time;
            }

            void CheckArgument(float time, string methodName)
            {
                if (time < 0)
                {
                    throw new System.ArgumentOutOfRangeException("Value time can not be less than 0 in " + methodName);
                }
            }

            public void Start(float duration)
            {
                if (this.started) return;
                this.duration = duration;
                this.started = true;
                this.stopped = false;
                this.time = UnityEngine.Time.timeSinceLevelLoad + duration;
                lastTime = this.time;
            }

            public void Start()
            {
                Start(this.duration);
            }

            public void Reset()
            {
                started = false;
                stopped = false;
            }

            public float CurrentTime
            {
                get
                {
                    if (stopped)
                    {
                        return lastTime;
                    }
                    else if (started)
                    {
                        lastTime = UnityEngine.Mathf.Max(0, time - UnityEngine.Time.timeSinceLevelLoad);
                        return lastTime;
                    }
                    else
                    {
                        return duration;
                    }
                }
            }

            public bool Started { get { return started; } }
            public bool Stopped { get { return stopped; } }

            public bool expired
            {
                get
                {
                    return CurrentTime <= 0 && duration > 0;
                }
            }

            public float GetDuration()
            {
                return duration;
            }

            public void SetDuration(float duration)
            {
                this.duration = duration;
            }

            public void Stop()
            {
                stopped = true;
            }

            public override string ToString()
            {
                return GetFormatted(CurrentTime);
            }

            public System.TimeSpan ToTimeSpan()
            {
                return GetTimeSpan(CurrentTime);
            }
        }

        public struct AmountOfType<T>
        {
            public T key { get; set; }
            public int amount { get; set; }

            public AmountOfType(T key, int amount)
                : this()
            {
                this.key = key;
                this.amount = amount;
            }

        }

    }
}
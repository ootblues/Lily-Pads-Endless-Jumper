using UnityEngine;
using System.Collections;

public class TargetGenerator : MonoBehaviour
{
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

    public EnterPadTrigger nextPadTrigger { get; private set; }
    public EnterPadTrigger currentPadTrigger { get; private set; }
    public Vector3 prefabScale { get { return m_PadPrefab.transform.localScale; } }

    System.Random m_Rnd = null;
    public float baseScale = 1.0f;

    void Start()
    {
        m_Rnd = new System.Random(m_Seed);
        //StartCoroutine(TestPlacement());
        m_LastPosition = m_StartPadGO.transform.position;
    }

    void OnEnable()
    {
        EnterPadTrigger.OnPadStopped -= EnterPadTrigger_OnPadStopped;
        EnterPadTrigger.OnPadStopped += EnterPadTrigger_OnPadStopped;
    }

    void OnDisable()
    {
        EnterPadTrigger.OnPadStopped -= EnterPadTrigger_OnPadStopped;
    }

    void EnterPadTrigger_OnPadStopped(EnterPadTrigger padTrigger)
    {
        StartCoroutine(NextPlacement(padTrigger));
    }

    IEnumerator NextPlacement(EnterPadTrigger padTrigger)
    {
        yield return null;
        m_LastPosition = padTrigger.transform.position;
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

            if(px > -m_LevelWidth && px < m_LevelWidth)
            {
                placing = false;
                newPosition = new Vector3(px, py, pz);
                nextPadTrigger = Instantiate(m_PadPrefab).GetComponent<EnterPadTrigger>();
                nextPadTrigger.transform.localScale = prefabScale * baseScale;
                nextPadTrigger.transform.position = newPosition;
                m_PositionBetweenTransforms.secondTransform = nextPadTrigger.transform;
                currentPadTrigger = m_PositionBetweenTransforms.firstTransform.GetComponent<EnterPadTrigger>();
            }
        }

        m_LastPosition = newPosition;
    }
}

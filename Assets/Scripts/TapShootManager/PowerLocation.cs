using UnityEngine;
using System.Collections;

public class PowerLocation : MonoBehaviour
{
    [SerializeField]
    AimerHandler m_AimerHandler = null;
    [SerializeField]
    Transform m_IndicatorTransform = null;

    [SerializeField]
    float m_MaximumDistance = 2.0f;

    [SerializeField]
    float m_Speed = 2.0f;

    [SerializeField]
    float m_Time = 0.0f;

    [SerializeField]
    float m_StartScale = 0.5f;

    [SerializeField]
    float m_PowerBase = 0.0f;

    void Update ()
    {
        m_Time += Time.deltaTime;
        m_PowerBase = Mathf.PingPong(m_Time * m_Speed, this.m_MaximumDistance);// - (this.m_MaximumDistance * 0.5f);

        Quaternion rotation = AimerHandler.RotationFromAngle(m_AimerHandler.angle);
        transform.rotation = rotation;

        //Vector3 scale = m_IndicatorTransform.localScale;
        //scale.z = m_StartScale * m_PowerBase;
        //m_IndicatorTransform.localScale = scale;
        //m_IndicatorTransform.localPosition = m_IndicatorTransform.forward * m_StartScale * m_PowerBase;
        //m_IndicatorTransform.rotation = rotation;
    }
}

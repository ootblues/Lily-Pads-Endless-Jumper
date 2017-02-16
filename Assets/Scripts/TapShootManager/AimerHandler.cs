using UnityEngine;
using System.Collections;


public enum LauncherState
{
    None,
    PingPong,
    ChargePower,
    Release
}

public class AimerHandler : MonoBehaviour
{
    [SerializeField]
    float m_Speed = 2.0f;
    [SerializeField]
    float m_MaxAngle = 45.0f;

    public float time = 0.0f;
    public float angle = 0.0f;

    public TargetAngle targetAngle = null;
    public Transform ballTransform = null;

    public LauncherState launcherState = LauncherState.None;
    public Vector3 pingPongDirection { get; set; }
    Quaternion lookRotation { get; set; }
    public Quaternion pingPongRotation { get; set; }

    public Vector3 lockDirection { get; private set; }

    void Start ()
    {
	
	}

    void Update ()
    {
	    if(Input.GetMouseButtonDown(0))
        {
            // Stop ping pong
            //Debug.Log("Stop ping pong");
            launcherState = LauncherState.None;
        }
        else if(Input.GetMouseButton(0))
        {
            // Charge power
            //Debug.Log("Charge power");
            launcherState = LauncherState.ChargePower;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            // Release and fire
            //Debug.Log("Release and fire");
            launcherState = LauncherState.PingPong;
        }

        switch (launcherState)
        {
            case LauncherState.None:
                break;
            case LauncherState.PingPong:
                CalculateRotation(Time.deltaTime);
                Quaternion rotation = pingPongRotation * RotationFromAngle(targetAngle.angle);
                transform.rotation = rotation;
                break;
            case LauncherState.ChargePower:
                break;
            case LauncherState.Release:
                break;
            default:
                break;
        }
    }

    public void CalculateRotation(float deltaTime)
    {
        this.time += deltaTime;
        this.angle = Mathf.PingPong(this.time * this.m_Speed, this.m_MaxAngle) - (this.m_MaxAngle * 0.5f);

        var rackPosition = transform.position;
        rackPosition.y = ballTransform.position.y;
        pingPongDirection = rackPosition - ballTransform.position;

        var currentRotation = RotationFromAngle(angle);
        lookRotation = Quaternion.LookRotation(pingPongDirection);

        lockDirection = currentRotation * pingPongDirection;
        pingPongRotation = currentRotation * lookRotation;
    }

    public static Quaternion RotationFromAngle(float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.up);
    }
}

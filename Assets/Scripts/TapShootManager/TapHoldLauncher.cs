using UnityEngine;
using System.Collections;


namespace LilyPadsEndlessJumper.TapShootManager
{
    [RequireComponent(typeof(Rigidbody))]
    public class TapHoldLauncher : MonoBehaviour
    {
        //[SerializeField]
        //float m_AngleSpeed = 2.0f;

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Stop ping pong
                //Debug.Log("Stop ping pong");
            }
            else if (Input.GetMouseButton(0))
            {
                // Charge power
                //Debug.Log("Charge power");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // Release and fire
                //Debug.Log("Release and fire");
            }
        }
    }
}
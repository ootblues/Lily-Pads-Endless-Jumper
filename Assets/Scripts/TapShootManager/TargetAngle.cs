using UnityEngine;
using System.Collections;

public class TargetAngle : MonoBehaviour
{
    public float angle = 0.0f;

    public Transform targetTransform = null;

    void Update ()
    {
        // the vector that we want to measure an angle from
        Vector3 referenceForward = Vector3.forward; /* some vector that is not Vector3.up */
                                   // the vector perpendicular to referenceForward (90 degrees clockwise)
                                   // (used to determine if angle is positive or negative)
        Vector3 referenceRight = Vector3.Cross(Vector3.up, referenceForward);
        // the vector of interest
        Vector3 newDirection = targetTransform.position - transform.position; /* some vector that we're interested in */
                               // Get the angle in degrees between 0 and 180
        float newAngle = Vector3.Angle(newDirection, referenceForward);
        // Determine if the degree value should be negative.  Here, a positive value
        // from the dot product means that our vector is on the right of the reference vector   
        // whereas a negative value means we're on the left.
        float sign = Mathf.Sign(Vector3.Dot(newDirection, referenceRight));
        float finalAngle = sign * newAngle;


        angle = finalAngle;// SignedAngleBetween(targetTransform.position, transform.position, Vector3.down);
        Quaternion rotation = AimerHandler.RotationFromAngle(angle);
        transform.rotation = rotation;
    }

    float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        // angle in [0,180]
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        // angle in [-179,180]
        float signed_angle = angle * sign;

        // angle in [0,360] (not used but included here for completeness)
        //float angle360 =  (signed_angle + 180) % 360;

        return signed_angle;
    }
}

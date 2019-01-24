using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetFilter))]
public class ProportionalNavigation : MonoBehaviour, IGuidanceLaw
{
    [Range(2,5)]
    public float Gain = 3;

    private Vector3 rangePrevious = Vector3.zero;
    private Vector3 range = Vector3.zero;

    private Vector3 losPrevious = Vector3.zero;
    private Vector3 los = Vector3.zero;

    // Calculate thrust.
    public void Calculate(float dt, Vector3 position, Vector3 velocity, Vector3 targetPosition, Vector3 targetVelocity, List<Vector3> acceleration)
    {
        rangePrevious = range;
        range = targetPosition - position;

        Vector3 dRange = (range - rangePrevious) / Time.deltaTime;
        Vector3 closingVelocity = velocity - targetVelocity;

        Debug.Assert(closingVelocity.magnitude - dRange.magnitude < 1e-4);

        losPrevious = los;
        los = Vector3.Normalize(range);
        Vector3 losRate = (los - losPrevious);

        Vector3 test0 = (dRange * range.magnitude - range * dRange.magnitude) / range.sqrMagnitude;
        Vector3 test1 = ((targetVelocity - velocity) / range.magnitude) + ((range * closingVelocity.magnitude) / range.sqrMagnitude);
        //Debug.Assert(Mathf.Approximately(Vector3.Dot(losRate, test0), 1f)); (1.11)
       // Debug.Assert(Mathf.Approximately(Vector3.Dot(test0, test1), 1f)); (1.11)

        float test2 = -Vector3.Dot(range, (targetVelocity - velocity)) / range.magnitude;
        float test3 = Vector3.Dot(range, closingVelocity) / range.magnitude;
        float test4 = Vector3.Dot(los, closingVelocity);
        //Debug.Assert(Mathf.Approximately(-dRange.magnitude, test2)); (1.12)
        //Debug.Assert(Mathf.Approximately(test2, test3)); (1.12)
        //Debug.Assert(Mathf.Approximately(test3, test4)); (1.12)

        float losAngle = Vector3.Angle(range, Vector3.right);
        float leadAngle = Vector3.Angle(velocity.normalized, range.normalized);
        float apsectAngle = Vector3.Angle(targetVelocity.normalized, range.normalized);

        //Debug.Assert(Mathf.Approximately(dRange.magnitude, velocityTarget.magnitude * Mathf.Cos(Mathf.Deg2Rad * apsectAngle) - velocity.magnitude * Mathf.Cos(Mathf.Deg2Rad * leadAngle))); // (2.4)

        float magnitude = Gain * closingVelocity.magnitude * losRate.magnitude;

        // lateral acceleration in perpendicular to the los.
        Vector3 losxz = targetPosition - position;
        losxz.y = 0;
        losxz = losxz.normalized;

        Vector3 direction = Vector3.Cross(Vector3.Cross(Vector3.up, losxz), velocity.normalized);

        Vector3 accel = magnitude * direction;

        Debug.DrawRay(position, accel, Color.red);

        acceleration.Add(accel);
    }
}

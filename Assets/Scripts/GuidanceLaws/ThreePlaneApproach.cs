using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetFilter))]
public class ThreePlaneApproach : MonoBehaviour, IGuidanceLaw
{
    [Range(3, 5)]
    public float Gain = 3;

    Vector3 rangePrevious = Vector3.zero;
    Vector3 range = Vector3.zero;

    Vector3 losPrevious = Vector3.zero;
    Vector3 los = Vector3.zero;

    // Update is called once per frame
    public void Calculate(float dt, Vector3 position, Vector3 velocity, Vector3 targetPosition, Vector3 targetVelocity, List<Vector3> acceleration)
    {
        rangePrevious = range;
        range = targetPosition - position;

        losPrevious = los;
        los = new Vector3(
            // xy-plane (30 a)
            Mathf.Atan(range.y / range.x),
            // xz-plane (30 b)
            Mathf.Atan(range.z / range.x),
            // yz-plane (30 c)
            Mathf.Atan(range.z / range.y));
        Vector3 losRate = (los - losPrevious);

        Vector3 relativeVelocity = targetVelocity - velocity;

        float losRateXY = (range.x * relativeVelocity.y - range.y * relativeVelocity.x) / (range.x * range.x + range.y * range.y);

        float distanceXY = Mathf.Sqrt(range.x * range.x + range.y * range.y);
        float distanceXZ = Mathf.Sqrt(range.x * range.x + range.z * range.z);
        float distanceYZ = Mathf.Sqrt(range.y * range.y + range.z * range.z);

        Vector3 closingVelocity = Vector3.zero;

        // xy-plane (31 a)
        if (distanceXY > 0)
        {
            closingVelocity.x = -(range.x * relativeVelocity.x + range.y * relativeVelocity.y) / distanceXY;
        }
        // xz-plane (31 b)
        if (distanceXZ > 0)
        {
            closingVelocity.y = -(range.x * relativeVelocity.x + range.z * relativeVelocity.z) / distanceXZ;
        }
        // yz-plane (31 c)
        if (distanceYZ > 0)
        {
            closingVelocity.z = -(range.x * relativeVelocity.x + range.z * relativeVelocity.z) / distanceYZ;
        }

        Vector3 accelerationCommand = new Vector3(
            // xy-plane (33 a)
            Gain * closingVelocity.x * losRate.x,
            // xz-plane (33 a)
            Gain * closingVelocity.y * losRate.y,
            // yz-plane (33 a)
            Gain * closingVelocity.z * losRate.z
            );

        Vector3 accelerationUnified = new Vector3(
            -accelerationCommand.x * Mathf.Sin(los.x) - accelerationCommand.y * Mathf.Sin(los.y),
            accelerationCommand.x * Mathf.Cos(los.x) - accelerationCommand.z * Mathf.Sin(los.z),
            accelerationCommand.y * Mathf.Cos(los.y) + accelerationCommand.z * Mathf.Cos(los.z)
            );

        float flightPathAngle = Mathf.Deg2Rad * Vector3.Angle(velocity.normalized, Vector3.ProjectOnPlane(velocity.normalized, Vector3.up));

        if (Mathf.Approximately(Vector3.ProjectOnPlane(velocity.normalized, Vector3.up).sqrMagnitude, 0f))
        {
            flightPathAngle = 0;
        }
        float accelerationPitch = (accelerationUnified.y + 0);
        accelerationPitch = Mathf.Abs(accelerationPitch);
        //accelerationPitch *= Mathf.Cos(flightPathAngle);

        float headingAngle = Mathf.Deg2Rad * Vector3.Angle(velocity.normalized, Vector3.ProjectOnPlane(velocity.normalized, Vector3.forward));
        float accelerationYaw = accelerationUnified.z * Mathf.Sin(Mathf.PI / 2.0f - headingAngle) - accelerationUnified.x * Mathf.Sin(headingAngle);

        Vector3 verticalAcceleration = accelerationPitch * Vector3.Cross(velocity.normalized, range.x > 0 ? Vector3.forward : Vector3.back);
        Vector3 lateralAcceleration = accelerationYaw * Vector3.Cross(velocity.normalized, verticalAcceleration.normalized);

        Debug.DrawRay(position, velocity, Color.green);
        Debug.DrawRay(position, verticalAcceleration, Color.red);
        //Debug.DrawRay(position, lateralAcceleration, Color.blue);
        Debug.DrawRay(position, Vector3.Cross(velocity.normalized, range.x > 0 ? Vector3.forward : Vector3.back));

        acceleration.Add(verticalAcceleration);
        acceleration.Add(lateralAcceleration);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetFilter))]
public class Mixed : MonoBehaviour, IGuidanceLaw
{
    // mathematically equivalent to a PN guidance law with a gain of N = 2.
    public void Calculate(float dt, Vector3 position, Vector3 velocity, Vector3 targetPosition, Vector3 targetVelocity, List<Vector3> acceleration)
    {
        Vector3 los = (targetPosition - position).normalized;

        float angle = Vector3.Angle(velocity.normalized, los);
        float magnitude = 2 * velocity.magnitude / Vector3.Distance(position, targetPosition) * Mathf.Sin(Mathf.Deg2Rad * angle);

        Plane p = new Plane(position, position + velocity.normalized, position + los);
        Vector3 lateral = Vector3.Cross(p.normal, velocity.normalized);
        Vector3 accel = magnitude * lateral;

        Debug.DrawRay(position, los, Color.green);
        Debug.DrawRay(position, velocity.normalized, Color.red);
        Debug.DrawRay(position, accel, Color.blue);

        acceleration.Add(accel);
    }
}

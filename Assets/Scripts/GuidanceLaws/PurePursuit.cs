using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurePursuit : MonoBehaviour
{
    public Rigidbody TargetBody;

    public float Gain = 3;

    // Update is called once per frame
    void FixedUpdate()
    {
        var rb = GetComponent<Rigidbody>();

        Vector3 velocity = rb.velocity;
        Vector3 position = rb.position;

        Vector3 velocityTarget = TargetBody.velocity;
        Vector3 positionTarget = TargetBody.position;

        Vector3 relativePosition = positionTarget - position;
        Vector3 relativeVelocity = velocityTarget - velocity;

        float tGo = -Vector3.Dot(relativePosition, relativeVelocity) / Vector3.Dot(relativeVelocity, relativeVelocity);

        Vector3 normal = Vector3.Cross(Vector3.Normalize(relativePosition), velocity);
        Vector3 unitLift = Vector3.Normalize(Vector3.Cross(velocity, normal));

        float liftMagnitude = (Gain * normal.magnitude) / Mathf.Max(tGo, 1f);

        rb.AddForce(liftMagnitude * unitLift, ForceMode.Impulse);
    }
}

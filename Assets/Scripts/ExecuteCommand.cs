using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Execute a series of guidance commands to steer a rigidbody.
/// </summary>
public class ExecuteCommand : MonoBehaviour
{
    [System.Serializable]
    public struct Guidance
    {
        public MonoBehaviour MonoBehaviour;
        public IGuidanceLaw Law
        {
            get
            {
                return MonoBehaviour as IGuidanceLaw;
            }
        }
        public bool ApplyAcceleration;
    }

    public Guidance[] Guidances = new Guidance[0];

    private List<Vector3> buffer = new List<Vector3>();
    private List<Vector3> acceleration = new List<Vector3>();

    private void FixedUpdate()
    {
        var rb = GetComponent<Rigidbody>();

        Vector3 position = rb.position;
        Vector3 targetPosition = GetComponent<TargetFilter>().Position;

        Vector3 velocity = rb.velocity;
        Vector3 targetVelocity = GetComponent<TargetFilter>().Velocity;

        acceleration.Clear();

        for (int guidanceIndex = 0; guidanceIndex < Guidances.Length; guidanceIndex++)
        {
            Guidance guidance = Guidances[guidanceIndex];
            if (guidance.ApplyAcceleration)
            {
                buffer.Clear();
                guidance.Law.Calculate(Time.fixedDeltaTime, position, velocity, targetPosition, targetVelocity, buffer);

                acceleration.AddRange(buffer);
            }
        }

        for (int accelerationIndex = 0; accelerationIndex < acceleration.Count; accelerationIndex++)
        {
            rb.AddForce(acceleration[accelerationIndex], ForceMode.Impulse);
        }
    }

    private void Reset()
    {
        var guidanceLaws = GetComponents<IGuidanceLaw>();
        Guidances = new Guidance[guidanceLaws.Length];
        for (int guidanceLawsIndex = 0; guidanceLawsIndex < guidanceLaws.Length; guidanceLawsIndex++)
        {
            Guidances[guidanceLawsIndex] = new Guidance()
            {
                MonoBehaviour = guidanceLaws[guidanceLawsIndex] as MonoBehaviour,
                ApplyAcceleration = false
            };
        }
    }
}

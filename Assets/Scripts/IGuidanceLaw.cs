using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGuidanceLaw
{
    void Calculate(float dt, Vector3 position, Vector3 velocity, Vector3 targetPosition, Vector3 targetVelocity, List<Vector3> acceleration);
}
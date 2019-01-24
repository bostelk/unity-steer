using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Start a rigidbody with an initial random velocity.
/// </summary>
public class StartVelocity : MonoBehaviour
{
    public float StartSpeed = 20;

    // Use this for initialization
    void Start()
    {
        var rb = GetComponent<Rigidbody>();

        Vector3 look = Random.onUnitSphere;
        look.y = Mathf.Abs(look.y);
        transform.rotation = Quaternion.LookRotation(look);
        rb.AddForce(StartSpeed * transform.forward, ForceMode.VelocityChange);
    }
}

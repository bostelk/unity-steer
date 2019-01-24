using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Steer a rigidbody in a circle around an origin.
/// </summary>
public class CircleSteer : MonoBehaviour
{
    [Tooltip("The origin to rotate around")]
    public Transform Origin;

    public float Speed = 20;

    private float radius = 0;

    private void Start()
    {
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(Speed * transform.forward, ForceMode.VelocityChange);

        radius = Vector3.Distance(rigidbody.position, Origin.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var rigidbody = GetComponent<Rigidbody>();

        Vector3 position = rigidbody.position;

        Vector3 tanget = Vector3.Cross(((position + rigidbody.velocity * Time.fixedDeltaTime * 0.5f) - Origin.position).normalized, Vector3.up);
        Vector3 impulse = Speed * Speed * tanget / radius - rigidbody.velocity;

        Vector3 vertical = Vector3.zero;
        vertical.y = 10 * Mathf.Sin(2f * Mathf.PI * Time.frameCount * Time.fixedDeltaTime);

        impulse += vertical;

        rigidbody.AddForce(impulse, ForceMode.Impulse);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFilter : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    public GameObject Target
    {
        get { return target; }
    }

    private Rigidbody rigidbody;

    public Vector3 Position
    {
        get
        {
            if (rigidbody)
            {
                return rigidbody.position;
            }
            else
            {
                return transform.position;
            }
        }   
    }

    public Vector3 Velocity
    {
        get
        {
            if (rigidbody)
            {
                return rigidbody.velocity;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    private void Awake()
    {
        if (Target != null)
        {
            SetTarget(Target);
        }
    }

    public void SetTarget(GameObject value)
    {
        target = value;
        rigidbody = value.GetComponent<Rigidbody>();
    }
}

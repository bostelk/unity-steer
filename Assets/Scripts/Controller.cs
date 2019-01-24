using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Tooltip("Key to toggle guidance on/off")]
    public KeyCode ToggleGuidanceKey;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(ToggleGuidanceKey))
        {
            var exe = GetComponent<ExecuteCommand>();
            if (exe)
            {
                exe.enabled = !exe.enabled;
            }
            var rb = GetComponent<Rigidbody>();
            if (rb)
            {
                rb.useGravity = !rb.useGravity;
            }
        }
    }
}

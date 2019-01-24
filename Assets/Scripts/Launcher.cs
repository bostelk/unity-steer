using UnityEngine;

/// <summary>
/// Create a prefab and set its target.
/// </summary>
public class Launcher : MonoBehaviour
{
    [Tooltip("The key to launch a prefab")]
    public KeyCode LaunchKey = KeyCode.Space;
    [Tooltip("The prefab to launch")]
    public GameObject Prefab;
    [Tooltip("The target to follow")]
    public GameObject Target;

    public float Radius = 10;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(LaunchKey))
        {
            GameObject instance = Instantiate(Prefab);

            TargetFilter targetFilter = instance.GetComponent<TargetFilter>();
            if (targetFilter != null)
            {
                targetFilter.SetTarget(Target);
            }

            Vector3 startPosition = transform.position;
            Vector2 unitCircle = Radius * Random.insideUnitCircle;
            startPosition.x += unitCircle.x;
            startPosition.z = unitCircle.y;

            instance.transform.position = startPosition;
        }
    }
}

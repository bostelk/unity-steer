using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    [Tooltip("Which layers cause destruction")]
    public LayerMask DestroyLayerMask = Physics.DefaultRaycastLayers;

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & DestroyLayerMask.value) != 0)
        {
            Destroy(gameObject);
        }
    }
}

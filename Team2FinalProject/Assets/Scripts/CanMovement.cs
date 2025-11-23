using UnityEngine;

public class CanMovement : MonoBehaviour
{
    private Vector3 startingPos;
    private Quaternion startingRot;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectManager.resetWorld.AddListener(Reset);
        rb = GetComponent<Rigidbody>();
        startingPos = transform.position;
        startingRot = transform.rotation;
    }

    public void Reset()
    {
        transform.position = startingPos;
        transform.rotation = startingRot;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            return;
        }
        Vector3 dir = (transform.position - collision.gameObject.transform.position).normalized;
        rb.AddForce(dir * 2, ForceMode.Impulse);
    }
}

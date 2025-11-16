using UnityEngine;
using System.Collections;

public class GolfBall : MonoBehaviour
{
    Rigidbody rb;
    Vector3 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        ObjectManager.resetWorld.AddListener(Reset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null || collision.collider == null || collision.gameObject.transform.parent == null) { return; }
        AnimationManager.StopAnimation();
        Debug.Log(collision.gameObject.transform.parent.gameObject.name);
        Vector3 dir = (transform.position - collision.gameObject.transform.position).normalized;
        rb.AddForce(dir * AnimationManager.getForce(), ForceMode.Impulse);
    }

    public void Reset()
    {
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        transform.position = startPos;
    }
}

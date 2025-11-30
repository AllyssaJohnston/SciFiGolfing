using UnityEngine;

public class Cylinder : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null || collision.collider == null || collision.gameObject.transform.parent == null 
            || collision.gameObject.layer == LayerMask.NameToLayer("Ball")) 
        { 
            return; 
        }
        AnimationManager.StopAnimation();
        Debug.Log(collision.gameObject.transform.parent.gameObject.name);
    }
}

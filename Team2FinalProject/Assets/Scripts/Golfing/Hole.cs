using UnityEngine;

public class Hole : MonoBehaviour
{
    private Vector3 startPos;
    bool setUp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setUp = true;
        startPos = transform.position;
        ObjectManager.resetWorld.AddListener(Reset);
    }

    private void Reset()
    {
        if (setUp)
        {
            transform.position = startPos;
           
        }
        
    }
}

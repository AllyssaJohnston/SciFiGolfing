using UnityEngine;

public class GolfBallViewCam : MonoBehaviour
{
    [SerializeField] GolfBall template;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.position = template.transform.position + Vector3.forward * 3;
    }

}

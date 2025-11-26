using UnityEngine;

public class ScrollBox : MonoBehaviour
{
    float startY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startY = transform.position.y;
        GameManager.gameModeChanged.AddListener(resetY);
    }

    void resetY()
    {
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
    }
}

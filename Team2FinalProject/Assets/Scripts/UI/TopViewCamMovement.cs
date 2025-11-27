using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TopViewCamMovement : MonoBehaviour
{
    private static Camera cam;

    private static float radius = 1f;
    private static Vector3 lookAtPos = Vector3.zero;

    private Vector3 startPos;


    private void Start()
    {
        cam = GetComponent<Camera>();
        startPos = gameObject.transform.position;
        radius = gameObject.transform.position.magnitude;
        GameManager.gameModeChanged.AddListener(switchMode);
    }

    private void Update()
    {
        if (GameManager.GetGameMode() == EGameMode.PLAY)
        {
            UpdateZoom();
            Move();
        }
    }

    private void switchMode()
    {
        gameObject.transform.position = startPos;
        lookAtPos = Vector3.zero;
        radius = gameObject.transform.position.magnitude;
    }

    private void UpdateZoom()
    {
        Vector3 additivePosition = Vector3.zero;
        List<GameObject> listToCheck = new List<GameObject>();
        foreach (GameObject obj in GolfBallManager.getGolfBallObjs())
        {
            listToCheck.Add(obj);
        }
        listToCheck.Add(World.GetRoot().gameObject);

        foreach (GameObject obj in listToCheck)
        {
            if (obj == null) { continue; }
            Vector2 screenPos = cam.WorldToViewportPoint(obj.transform.position);
            additivePosition += obj.transform.position;
            Debug.Log(screenPos);
            if (screenPos.x < 0)
            {
                radius = Mathf.Max(radius, (obj.transform.position - lookAtPos).magnitude);
            }
            if (screenPos.x > 1)
            {
                radius = Mathf.Max(radius, (obj.transform.position - lookAtPos).magnitude);
            }
            if (screenPos.y < 0)
            {
                radius = Mathf.Max(radius, (obj.transform.position - lookAtPos).magnitude);
            }
            if (screenPos.y > 1)
            {
                radius = Mathf.Max(radius, (obj.transform.position - lookAtPos).magnitude);
            }
        }
        lookAtPos = additivePosition / (float)listToCheck.Count;
    }

    private void Move()
    {
        // move out from pivot
        transform.localPosition = lookAtPos - (radius * transform.forward);
    }
}

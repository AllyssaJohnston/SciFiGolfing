using UnityEngine;

public class AnimatePrimitive : MonoBehaviour
{
    [Header("Play Settings")]
    public float minDegree = 0f;
    public float maxDegree = 90f;
    public float degreePerSec = 45f;
    private float startingDegreePerSec = 45f;
    
    [Header("Editor and Play Settings")]
    public float startDegree = 0f;
    public EAxis moveAxis = EAxis.Z;
    public Vector3 rotAxis = Vector3.up;
    public float radius = 1f;
    public Vector3 pivot;

    private float degree = 0f;
    private bool init = false;

    private void Start()
    {
        degree = startDegree;
        startingDegreePerSec = degreePerSec;
    }

    private void Update()
    {
        transform.localPosition = pivot;
        degree += degreePerSec * Time.deltaTime;
        transform.localRotation *= Quaternion.AngleAxis(degreePerSec * Time.deltaTime, rotAxis);
        if ((degree > maxDegree) || (degree < minDegree))
        {
            degreePerSec *= -1f;
            degree += degreePerSec * Time.deltaTime;
        }

        // move out from pivot
        transform.localPosition += (radius * GetForwardDirection(degree));
    }

    public void UpdateRotationInEditor()
    {
        if (init == false) // initialize
        {
            degree = startDegree;
        }
        transform.localPosition = pivot;
        transform.localRotation *= Quaternion.AngleAxis(degree - startDegree, rotAxis);
        degree = startDegree;
        // move out from pivot
        transform.localPosition += (radius * GetForwardDirection(degree));
    }

    public void ResetNode()
    {
        // calc rotation to starting rotation
        float angleChange = degree - startDegree;
        transform.localPosition = pivot;
        transform.localRotation *= Quaternion.AngleAxis(angleChange, rotAxis);

        // move out from pivot
        transform.localPosition += (radius * GetForwardDirection(startDegree));
        degree = startDegree; // reset degree
        degreePerSec = startingDegreePerSec;
    }

    // calculate a forward direction based on the direction the user said they want the object to move
    private Vector3 GetForwardDirection(float degree)
    {
        float rad = Mathf.PI * degree / 180;
        Vector3 forwardDir = Vector3.one;
        switch (moveAxis)
        {
            case EAxis.X:
                // only have components in x and y direction
                forwardDir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
                break;
            case EAxis.Y:
                // only have components in x and z direction
                forwardDir = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));
                break;
            case EAxis.Z:
                // only have components in y and z direction
                forwardDir = new Vector3(0, Mathf.Cos(rad), Mathf.Sin(rad));
                break;
            default:
                break;
        }
        return forwardDir;
    }
}

using UnityEngine;

public enum EMouseButton
{
    LEFT = 0,
    RIGHT = 1,
    MIDDLE = 2
}

public class CameraMovement : MonoBehaviour
{
    private static CameraMovement instance;

    public float degreePerSec = 15f;
    public float zoomPerSec = 20f;
    public float transPerSec = 15f;

    private static float startRadius = 1f;
    private static float radius = 1f;
    private static Vector3 lookAtPos = Vector3.zero;
    private static Vector3 startingAngle = Vector3.zero;
    private static Vector3 angle = Vector3.zero;

    private static Vector2 mousePos = Vector3.zero;
    private static Vector2 lastMousePos = Vector3.zero;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        angle.y = transform.localRotation.eulerAngles.x;
        startingAngle.x = angle.x;
        startingAngle.y = angle.y;
        radius = Mathf.Pow(Mathf.Pow(transform.localPosition.z, 2) + Mathf.Pow(transform.localPosition.y, 2), .5f);
        startRadius = radius;
    }

    private void Update()
    {
        // orbiting
        Vector3 angleChangeVect = Vector3.zero;
        if (Input.GetMouseButtonDown((int)EMouseButton.LEFT))
        {
            // just get the mouse position
            mousePos = Input.mousePosition;
        }
        else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton((int)EMouseButton.LEFT))
        {
            // get the mouse position and compare it to the last mouse position
            mousePos = Input.mousePosition;
            angleChangeVect = (mousePos - lastMousePos) * degreePerSec * Time.deltaTime;
            angleChangeVect.y *= -1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            angleChangeVect.x = -1 * degreePerSec * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            angleChangeVect.x = degreePerSec * Time.deltaTime;
        }


        // tracking / panning
        Vector3 transPos = Vector3.zero;
        if (Input.GetMouseButtonDown((int)EMouseButton.RIGHT))
        {
            // just get the mouse position
            mousePos = Input.mousePosition;
        }
        else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton((int)EMouseButton.RIGHT))
        {
            // get the mouse position and compare it to the last mouse position
            mousePos = Input.mousePosition;
            transPos = (mousePos - lastMousePos) * transPerSec * Time.deltaTime;

        }


        // scrolling
        float zoom = 0f;
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKey(KeyCode.LeftAlt) && (scrollWheel != 0))
        {
            // get the mouse position and compare it to the last mouse position
            scrollWheel = (scrollWheel > 0) ? -1 : 1;
            zoom = zoomPerSec * Time.deltaTime * scrollWheel;
            Debug.Log(scrollWheel + " " + zoom);
        }

        radius += zoom;
        lookAtPos += transPos;
        angle += angleChangeVect;
        Move();
        lastMousePos = mousePos;
    }

    public void Reset()
    {
        // go back to lookat pos
        lookAtPos = Vector3.zero;
        transform.localPosition = lookAtPos;
        angle = startingAngle;
        radius = startRadius;
        Move();
    }

    // move & rotate the camera based on the current angle, radius, and lookAtPos
    private void Move()
    {
        transform.localRotation = Quaternion.identity;
        transform.localRotation *= Quaternion.AngleAxis(-angle.x, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(angle.y, Vector3.right);

        // move out from pivot
        transform.localPosition = lookAtPos - (radius * transform.forward);
    }

    public static void UpdateLookAt(EAxis axis, float value)
    {
        switch (axis)
        {
            case EAxis.X: lookAtPos.x = value; break;
            case EAxis.Y: lookAtPos.y = value; break;
            case EAxis.Z: lookAtPos.z = value; break;
            default: Debug.Log("unrecognized axis " + axis); break;
        }
    }

    public static Vector3 GetPosition() { return lookAtPos; }

}

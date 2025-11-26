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
    public float startRadius = 1f;
    public float maxTransVal = 20f;

    private static float radius = 1f;
    private static Vector3 startLookAtPos = Vector3.zero;
    private static Vector3 lookAtPos = Vector3.zero;
    private static Vector3 startingAngle = Vector3.zero;
    private static Vector3 angle = Vector3.zero;

    private static Vector2 mousePos = Vector3.zero;
    private static Vector2 lastMousePos = Vector3.zero;

    public Vector3 playModePos = new Vector3(0, 10, 300);

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
        angle.x = -transform.localRotation.eulerAngles.y;
        angle.y = transform.localRotation.eulerAngles.x;
        startingAngle = angle;
        radius = startRadius;
        lookAtPos = transform.localPosition + radius * transform.forward;
        startLookAtPos = lookAtPos;
        GameManager.gameModeChanged.AddListener(gameModeChanged);
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
        SetLookAt(transPos);
        angle += angleChangeVect;
        Move();
        lastMousePos = mousePos;
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

    public static void SetLookAt(Vector3 transPos)
    {
        Vector3 oldLookAt = lookAtPos;
        lookAtPos += transPos;
        lookAtPos = new Vector3(Mathf.Max(-instance.maxTransVal, lookAtPos.x), Mathf.Max(-instance.maxTransVal, lookAtPos.y), Mathf.Max(-instance.maxTransVal, lookAtPos.z));
        lookAtPos = new Vector3(Mathf.Min(instance.maxTransVal, lookAtPos.x), Mathf.Min(instance.maxTransVal, lookAtPos.y), Mathf.Min(instance.maxTransVal, lookAtPos.z));
        if (oldLookAt != lookAtPos)
        {
            SliderManager.ResetCameraSliders();
        }
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

    public void Reset()
    {
        // go back to lookat pos
        lookAtPos = startLookAtPos;
        transform.localPosition = lookAtPos;
        angle = startingAngle;
        radius = startRadius;
        Move();
    }

    public static float getMaxTransVal() { return instance.maxTransVal; }

    private static void gameModeChanged()
    {
        EGameMode gameMode = GameManager.GetGameMode();

        switch(gameMode)
        {
            case EGameMode.SETUP:
                instance.Reset();
                break;
            case EGameMode.PLAY:
                instance.Reset();
                lookAtPos = Vector3.right * 20;
                radius = (instance.playModePos - lookAtPos).magnitude;
                angle = new Vector3(0, startingAngle.y, 0);
                instance.transform.position = instance.playModePos;
                break;
        }
    }
}

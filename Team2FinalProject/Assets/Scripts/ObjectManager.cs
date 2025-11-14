using UnityEngine;
using UnityEngine.Events;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager instance;
    private static SceneNode curSceneObj; // SceneNode of the selected scene node
    public static UnityEvent curObjectChanged;
    public static UnityEvent curSceneObjectValuesChanged;
    public GameObject axes;
    public GameObject forwardLine;
    

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        curObjectChanged = new UnityEvent();
        curSceneObjectValuesChanged = new UnityEvent();
        curSceneObj = World.GetRoot();
        Debug.Assert(curSceneObj != null);
    }

    public static void SetCurObject(SceneNode obj) 
    {
        curSceneObj = obj;
        curObjectChanged.Invoke();
    }

    public static SceneNode GetCurObject() { return curSceneObj; }

    // get a value of the scene node
    public static float GetCurObjectValue(EAxis axis)
    {
        switch (axis)
        {
            case EAxis.X: return curSceneObj.rotation.x;
            case EAxis.Y: return curSceneObj.rotation.y;
            case EAxis.Z: return curSceneObj.rotation.z;
            default: Debug.Log("unrecognized axis " + axis); return 0f;
        } 
    }

    // set a value of the scene node
    public static void SetCurObjectValue(EAxis axis, float value)
    {
        float deltaAngle = 0f;
        SceneNode nodeScript = curSceneObj.GetComponent<SceneNode>();
        switch (axis)
        {
            case EAxis.X:
                deltaAngle = value - nodeScript.rotation.x;
                curSceneObj.transform.localRotation *= QuatScript.UpdateRotate(deltaAngle, EAxis.X);
                nodeScript.rotation.x = value;
                break;
            case EAxis.Y:
                deltaAngle = value - nodeScript.rotation.y;
                curSceneObj.transform.localRotation *= QuatScript.UpdateRotate(deltaAngle, EAxis.Y);
                nodeScript.rotation.y = value;
                break;
            case EAxis.Z:
                deltaAngle = value - nodeScript.rotation.z;
                curSceneObj.transform.localRotation *= QuatScript.UpdateRotate(deltaAngle, EAxis.Z);
                nodeScript.rotation.z = value;
                break;
            default:
                Debug.Log("unrecognized axis " + axis);
                break;
        }

        curSceneObjectValuesChanged.Invoke();
    }
}

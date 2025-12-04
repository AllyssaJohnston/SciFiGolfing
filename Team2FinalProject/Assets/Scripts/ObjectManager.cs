using UnityEngine;
using UnityEngine.Events;

public enum ELastChanged
{
    SCENE_NODE = 0,
    HOLE
}

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager instance;

    private static ELastChanged lastChanged = ELastChanged.SCENE_NODE;

    private static SceneNode curSceneObj; // SceneNode of the selected scene node
    public static UnityEvent curObjectChanged;
    public static UnityEvent curSceneObjectValuesChanged;

    private static GameObject curHoleObj;
    public static UnityEvent curHoleChanged;

    public static UnityEvent backToSetUp;
    public static UnityEvent resetWorld;

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
        curHoleChanged = new UnityEvent();
        backToSetUp = new UnityEvent();
        resetWorld = new UnityEvent();
    }

    public static void SetCurObject(SceneNode obj) 
    {
        curSceneObj = obj;
        lastChanged = ELastChanged.SCENE_NODE;
        curObjectChanged.Invoke();
    }

    public static void SetCurHoleObject(GameObject obj)
    {
        curHoleObj = obj;
        lastChanged = ELastChanged.HOLE;
        curHoleChanged.Invoke();
    }

    public static SceneNode GetCurObject() { return curSceneObj; }
    public static GameObject GetCurHoleObject() { return curHoleObj; }

    public static ELastChanged GetLastChanged() {  return lastChanged; }

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

        lastChanged = ELastChanged.SCENE_NODE;
        curSceneObjectValuesChanged.Invoke();
    }

    // get a value of the hole
    public static float GetCurHoleObjectValue(EAxis axis)
    {
        switch (axis)
        {
            case EAxis.X: return curHoleObj.transform.position.x;
            case EAxis.Z: return curHoleObj.transform.position.z;
            default: Debug.Log("unrecognized axis " + axis); return 0f;
        }
    }

    // set a value of the hole
    public static void SetCurHoleObjectValue(EAxis axis, float value)
    {
        switch (axis)
        {
            case EAxis.X:
                curHoleObj.transform.position = new Vector3(value, curHoleObj.transform.position.y, curHoleObj.transform.position.z);
                break;
            case EAxis.Z:
                curHoleObj.transform.position = new Vector3(curHoleObj.transform.position.x, curHoleObj.transform.position.y, value);
                break;
            default:
                Debug.Log("unrecognized axis " + axis);
                break;
        }
        lastChanged = ELastChanged.HOLE;
    }

    public static void BackToSetUp() { backToSetUp.Invoke();  }

    public static void Reset() 
    { 
        resetWorld.Invoke();
        lastChanged = ELastChanged.SCENE_NODE;
    }
}

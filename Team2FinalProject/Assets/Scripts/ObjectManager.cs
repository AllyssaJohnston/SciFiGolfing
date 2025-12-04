using UnityEngine;
using UnityEngine.Events;

public class ObjectManager : MonoBehaviour
{
    private static ObjectManager instance;

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
        curObjectChanged.Invoke();
    }

    public static void SetCurHoleObject(GameObject obj)
    {
        curHoleObj = obj;
        curHoleChanged.Invoke();
    }

    public static SceneNode GetCurObject() { return curSceneObj; }

    public static GameObject GetCurHoleObject() { return curHoleObj; }

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
        switch (axis)
        {
            case EAxis.X:
                deltaAngle = value - curSceneObj.rotation.x;
                curSceneObj.transform.localRotation *= QuatScript.UpdateRotate(deltaAngle, EAxis.X);
                curSceneObj.rotation.x = value;
                break;
            case EAxis.Y:
                deltaAngle = value - curSceneObj.rotation.y;
                curSceneObj.transform.localRotation *= QuatScript.UpdateRotate(deltaAngle, EAxis.Y);
                curSceneObj.rotation.y = value;
                break;
            case EAxis.Z:
                deltaAngle = value - curSceneObj.rotation.z;
                curSceneObj.transform.localRotation *= QuatScript.UpdateRotate(deltaAngle, EAxis.Z);
                curSceneObj.rotation.z = value;
                break;
            default:
                Debug.Log("unrecognized axis " + axis);
                break;
        }
        curSceneObjectValuesChanged.Invoke();
    }

    // set a value of the scene node
    public static void ChangeCurObjectValueBy(EAxis axis, float deltaAngle)
    {
        curSceneObj.transform.localRotation *= QuatScript.UpdateRotate(deltaAngle, axis);
        switch (axis)
        {
            case EAxis.X:
                curSceneObj.rotation.x += deltaAngle;
                break;
            case EAxis.Y:
                curSceneObj.rotation.y += deltaAngle;
                break;
            case EAxis.Z:
                curSceneObj.rotation.z += deltaAngle;
                break;
            default:
                Debug.Log("unrecognized axis " + axis);
                break;
        }
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
    }

    public static void BackToSetUp() { backToSetUp.Invoke();  }

    public static void Reset() { resetWorld.Invoke(); }
}

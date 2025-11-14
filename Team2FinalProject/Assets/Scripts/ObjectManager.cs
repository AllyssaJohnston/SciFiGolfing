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
    public static float GetCurObjectValue(EModifierType attribute, EAxis axis)
    {
        switch (attribute)
        {
            case EModifierType.POSITION:
                switch (axis)
                {
                    case EAxis.X: return curSceneObj.transform.localPosition.x;
                    case EAxis.Y: return curSceneObj.transform.localPosition.y;
                    case EAxis.Z: return curSceneObj.transform.localPosition.z;
                    default: Debug.Log("unrecognized axis " + axis); break;
                }
                break;

            case EModifierType.ROTATION:
                switch (axis)
                {
                    case EAxis.X: return curSceneObj.rotation.x;
                    case EAxis.Y: return curSceneObj.rotation.y;
                    case EAxis.Z: return curSceneObj.rotation.z;
                    default: Debug.Log("unrecognized axis " + axis); break;
                }
                break;

            case EModifierType.SCALE:
                switch (axis)
                {
                    case EAxis.X: return curSceneObj.transform.localScale.x;
                    case EAxis.Y: return curSceneObj.transform.localScale.y;
                    case EAxis.Z: return curSceneObj.transform.localScale.z;
                    default: Debug.Log("unrecognized axis " + axis); break;
                }
                break;

            default:
                Debug.Log("unrecognized attribute " + attribute);
                break;
        }
        return 0;
    }

    // set a value of the scene node
    public static void SetCurObjectValue(EModifierType attribute, EAxis axis, float value)
    {
        switch (attribute)
        {
            case EModifierType.POSITION:
                switch (axis)
                {
                    case EAxis.X:
                        curSceneObj.transform.localPosition = new Vector3(value, curSceneObj.transform.localPosition.y, curSceneObj.transform.localPosition.z);
                        break;
                    case EAxis.Y:
                        curSceneObj.transform.localPosition = new Vector3(curSceneObj.transform.localPosition.x, value, curSceneObj.transform.localPosition.z);
                        break;
                    case EAxis.Z:
                        curSceneObj.transform.localPosition = new Vector3(curSceneObj.transform.localPosition.x, curSceneObj.transform.localPosition.y, value);
                        break;
                    default:
                        Debug.Log("unrecognized axis " + axis);
                        break;
                }
                break;

            case EModifierType.ROTATION:
                SetCurObjectRot(attribute, axis, value);
                break;

            case EModifierType.SCALE:
                switch (axis)
                {
                    case EAxis.X:
                        curSceneObj.transform.localScale = new Vector3(value, curSceneObj.transform.localScale.y, curSceneObj.transform.localScale.z);
                        break;
                    case EAxis.Y:
                        curSceneObj.transform.localScale = new Vector3(curSceneObj.transform.localScale.x, value, curSceneObj.transform.localScale.z);
                        break;
                    case EAxis.Z:
                        curSceneObj.transform.localScale = new Vector3(curSceneObj.transform.localScale.x, curSceneObj.transform.localScale.y, value);
                        break;
                    default:
                        Debug.Log("unrecognized axis " + axis);
                        break;
                }
                break;
            default:
                Debug.Log("unrecognized attribute " + attribute);
                break;
        }

        curSceneObjectValuesChanged.Invoke();
    }

    // set the scene node rotation
    private static void SetCurObjectRot(EModifierType attribute, EAxis axis, float value)
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
    }
}

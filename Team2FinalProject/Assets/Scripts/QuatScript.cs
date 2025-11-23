using UnityEngine;

public class QuatScript : MonoBehaviour
{
    public static Quaternion UpdateRotate(float deltaAngle, EAxis axis)
    {
        Vector3 v = Vector3.zero;
        switch (axis)
        {
            case EAxis.X:
                v = Vector3.right;
                break;
            case EAxis.Y:
                v = Vector3.up;
                break;
            case EAxis.Z:
                v = Vector3.forward;
                break;
            default:
                Debug.Log("unrecognized axis");
                break;
        }
        return Quaternion.AngleAxis(deltaAngle, v);
    }

    public static Quaternion GetRotation(Vector3 direction)
    {
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction).normalized;
        float angle = Vector3.Angle(Vector3.up, direction);

        return Quaternion.AngleAxis(angle, rotationAxis);
    }
}

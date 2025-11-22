using UnityEngine;

public enum EAxis
{
    X = 0,
    Y,
    Z
}

public class XYZSliderScript : SliderScript
{
    public EAxis axis;
    public bool cameraSlider = false;

    // called at start
    protected override void subSetUp()
    {
        if (cameraSlider)
        {
            slider.minValue = -1 * CameraMovement.getMaxTransVal();
            slider.maxValue = CameraMovement.getMaxTransVal();
            resetData();
        }
    }

    protected override void SetPrefix()
    {
        switch (axis)
        {
            case EAxis.X: prefix = "X:"; break;
            case EAxis.Y: prefix = "Y:"; break;
            case EAxis.Z: prefix = "Z:"; break;
            default: Debug.Log("unrecognized value " + axis);
                break;
        }
    }

    // set the slider's values to the newly selected object's values
    public override void resetData()
    {
        float value;
        if (cameraSlider)
        {
            Vector3 position = CameraMovement.GetPosition();
            value = position.x;
            switch (axis)
            {
                case EAxis.Y:
                    value = position.y; break;
                case EAxis.Z:
                    value = position.z; break;
            }
        }
        else
        {
            value = ObjectManager.GetCurObjectValue(axis);
        }
        slider.value = value;
        lastValue = value;
        UpdateLabel();
    }


    //user has moved the slider, update its label
    //update the scene node's attributes or camera attributes
    public override void SliderMoved(float value)
    {
        GameObject curSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (curSelected != null)
        {
            UpdateLabel();
            if (cameraSlider)
            {
                CameraMovement.UpdateLookAt(axis, value);
            }
            else
            {
                ObjectManager.SetCurObjectValue(axis, value);
            }
        }
    }
}

using UnityEngine;

public enum ESliderType
{
    CAMERA,
    NODE,
    DIFFUSE,
    POINT,
    HOLE
}

public enum EAxis
{
    X = 0,
    Y,
    Z
}

public class XYZSliderScript : SliderScript
{
    public EAxis axis;
    public ESliderType type = ESliderType.NODE;

    // called at start
    protected override void subSetUp()
    {
        slider.value = 0f;
        lastValue = slider.value;
        if (type == ESliderType.CAMERA)
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
        
        float value = 0f;
        Vector3 position;
        switch (type)
        {
            case ESliderType.CAMERA:
       
                position = CameraMovement.GetPosition();
                value = position.x;
                switch (axis)
                {
                    case EAxis.Y:
                        value = position.y; break;
                    case EAxis.Z:
                        value = position.z; break;
                }
                break;

            case ESliderType.NODE:
                gameObject.transform.parent.gameObject.SetActive(showRotSlider());
                value = ObjectManager.GetCurObjectValue(axis);
                break;

            case ESliderType.DIFFUSE:
                position = LightManager.GetDiffuseLightPosition();
                value = position.x;
                switch (axis)
                {
                    case EAxis.Y:
                        value = position.y; break;
                    case EAxis.Z:
                        value = position.z; break;
                }
                break;

            case ESliderType.POINT:
                position = LightManager.GetPointLightPosition();
                value = position.x;
                switch (axis)
                {
                    case EAxis.Y:
                        value = position.y; break;
                    case EAxis.Z:
                        value = position.z; break;
                }
                break;

            case ESliderType.HOLE:
                value = ObjectManager.GetCurHoleObjectValue(axis);
                break;
        }
        slider.value = value;
        lastValue = value;
        UpdateLabel();
        SliderManager.SliderMoved();
    }


    //user has moved the slider, update its label
    //update the scene node's attributes or camera attributes
    public override void SliderMoved(float value)
    {
        GameObject curSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (curSelected != null)
        {
            UpdateLabel();
            switch (type)
            {
                case ESliderType.CAMERA:
                    CameraMovement.UpdateLookAt(axis, value);
                    break;
                case ESliderType.NODE:
                    ObjectManager.SetCurObjectValue(axis, value);
                    break;
                case ESliderType.DIFFUSE:
                    LightManager.UpdateDiffuseLightPosition(axis, value);
                    break;
                case ESliderType.POINT:
                    LightManager.UpdatePointLightPosition(axis, value);
                    break;
                case ESliderType.HOLE:
                    ObjectManager.SetCurHoleObjectValue(axis, value);
                    break;
            }
            SliderManager.SliderMoved();
        }
    }


    private bool showRotSlider()
    {
        Vector3 editable = ObjectManager.GetCurObject().editableAxes;
        switch (axis)
        {
            case EAxis.X:
                return editable.x == 1;
            case EAxis.Y:
                return editable.y == 1;
            case EAxis.Z:
                return editable.z == 1;
        }
        return false;
    }
}

using UnityEngine;

public enum ESliderAttribute
{
    RANGE = 0,
    FORCE,
    TIME
}

public class AttributeSliderScript : SliderScript
{
    public ESliderAttribute attribute;

    protected override void SetPrefix()
    {
        switch (attribute)
        {
            case ESliderAttribute.RANGE: prefix = "Range:"; break;
            case ESliderAttribute.FORCE: prefix = "Force:"; break;
            case ESliderAttribute.TIME: prefix = "Time:"; break;
            default:
                Debug.Log("unrecognized value " + attribute);
                break;
        }
    }

    // set the slider's values to the newly selected object's values
    public override void resetData()
    {
        float value = 0f;
        // get golf attributes
        //value = ObjectManager.GetCurObjectValue(axis);
        slider.value = value;
        lastValue = value;
        UpdateLabel();
    }

    public override void SliderMoved(float value)
    {

    }
}

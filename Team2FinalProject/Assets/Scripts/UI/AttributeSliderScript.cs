using UnityEngine;

public enum ESliderAttribute
{
    RANGE = 0,
    FORCE,
    SPEED
}

public class AttributeSliderScript : SliderScript
{

    void Start()
    {
        Debug.Assert(slider != null);
        resetData();
        SetPrefix();
        UpdateLabel();
        ObjectManager.curObjectChanged.AddListener(resetData);
    }


    public ESliderAttribute attribute;

    protected override void SetPrefix()
    {
        switch (attribute)
        {
            case ESliderAttribute.RANGE: prefix = "Range:"; break;
            case ESliderAttribute.FORCE: prefix = "Force:"; break;
            case ESliderAttribute.SPEED: prefix = "Speed:"; break;
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
        switch (attribute)
        {

            case ESliderAttribute.RANGE:
                value = AnimationManager.getRange();
                break;
            case ESliderAttribute.FORCE:
                value = AnimationManager.getForce();
                break;
            case ESliderAttribute.SPEED:
                value = AnimationManager.getSpeed();
                break;
            default:
                Debug.Log("unrecognized value " + attribute);
                break;
        }
        slider.value = value;
        lastValue = value;
        UpdateLabel();
    }

    public override void SliderMoved(float value)
    {
        switch (attribute)
        {

            case ESliderAttribute.RANGE:
                AnimationManager.setRange(value);
                break;
            case ESliderAttribute.FORCE:
                AnimationManager.setForce(value);
                break;
            case ESliderAttribute.SPEED:
                AnimationManager.setSpeed(value);
                break;
            default:
                Debug.Log("unrecognized value " + attribute);
                break;
        }
        UpdateLabel();
        
    }
}

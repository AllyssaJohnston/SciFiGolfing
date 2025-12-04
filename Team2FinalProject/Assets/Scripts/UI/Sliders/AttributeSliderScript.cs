using UnityEngine;

public enum ESliderAttribute
{
    RANGE = 0,
    FORCE,
    SPEED,
    DUP_ROTATION,
    DUP_FORCE
}

public class AttributeSliderScript : SliderScript
{

    public ESliderAttribute attribute;

    protected override void subSetUp()
    {
        resetData();
    }

    protected override void SetPrefix()
    {
        switch (attribute)
        {
            case ESliderAttribute.RANGE: prefix = "Range:"; break;
            case ESliderAttribute.FORCE: prefix = "Force:"; break;
            case ESliderAttribute.SPEED: prefix = "Speed:"; break;
            case ESliderAttribute.DUP_ROTATION: prefix = "Angle:"; break;
            case ESliderAttribute.DUP_FORCE: prefix = "Force:"; break;
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
            case ESliderAttribute.DUP_ROTATION:
                value = GolfBallManager.getDuplicationRotation();
                break;
            case ESliderAttribute.DUP_FORCE:
                value = GolfBallManager.getDuplicationForce();
                break;
            default:
                Debug.Log("unrecognized value " + attribute);
                break;
        }
        slider.value = value;
        lastValue = value;
        UpdateLabel();
        SliderManager.SliderMoved();
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
            case ESliderAttribute.DUP_ROTATION:
                GolfBallManager.setDuplicationRotation(value);
                break;
            case ESliderAttribute.DUP_FORCE:
                GolfBallManager.setDuplicationForce(value);
                break;
            default:
                Debug.Log("unrecognized value " + attribute);
                break;
        }
        UpdateLabel();
        SliderManager.SliderMoved();
    }
}

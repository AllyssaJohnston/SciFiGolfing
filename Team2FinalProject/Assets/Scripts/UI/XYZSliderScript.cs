using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum EAxis
{
    X = 0,
    Y,
    Z
}

public class XYZSliderScript : MonoBehaviour
{
    public EAxis axis;
    public TMP_Text label;
    private string prefix;
    public Slider slider;
    public bool singleSliderMode = false;
    [SerializeField] List<SliderMode> sliderModes = new List<SliderMode>();
    private float lastValue;
    private bool setUp = false;

    void Start()
    {
        Debug.Assert(slider != null);
        Debug.Assert(sliderModes.Count > 0);

        slider.value = 0f;
        lastValue = slider.value;
        SetPrefix();
        UpdateLabel();
        ObjectManager.curObjectChanged.AddListener(resetData);
    }

    private void Update()
    {
        if (!setUp)
        {
            resetData();
            setUp = true;
        }
    }

    private void SetPrefix()
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

    // the user has entered a new mode, update the slider to the currently selected object, and update the slider's values, parameters
    public void updateSliderMode(EModifierType type)
    {
        slider.minValue = sliderModes[(int)type].min;
        slider.maxValue = sliderModes[(int)type].max;

        resetData();
        UpdateLabel();
    }

    // set the slider's label to the value
    public void UpdateLabel() 
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            // Unity sometimes lets keyboard input adjust the slider. Internet didn't yield much insight on why this happens, 
            // so I'll just undo the slider update
            slider.value = lastValue;
            return;
        }
        label.text = prefix + " " + Math.Round(slider.value, 2, MidpointRounding.AwayFromZero).ToString();
        lastValue = slider.value;
    }

    // set the slider's values to the newly selected object's values
    public void resetData()
    {
        float value;
        if (singleSliderMode)
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
            value = ObjectManager.GetCurObjectValue(SliderManager.getCurSliderMode(), axis);
        }
        slider.value = value;
        lastValue = value;
        UpdateLabel();
    }
}

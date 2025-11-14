using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public enum EColorComp 
{
    R = 0,
    G = 1,
    B = 2,
}

public class RGBSliderScript : MonoBehaviour
{
    public EColorComp colorComp;
    public TMP_Text label;
    private string defaultLabel;
    public Slider slider;
    private float lastValue;
    private bool setUp = false;

    void Start()
    {
        Debug.Assert(slider != null);

        slider.value = 0f;
        lastValue = slider.value;
        SetDefaultLabel();
        UpdateLabel();
        ObjectManager.curPrimObjectChanged.AddListener(resetData);
    }

    private void Update()
    {
        if (!setUp)
        {
            resetData();
            setUp = true;
        }
    }


    private void SetDefaultLabel()
    {
        switch (colorComp)
        {
            case EColorComp.R: defaultLabel = "R:"; break;
            case EColorComp.G: defaultLabel = "G:"; break;
            case EColorComp.B: defaultLabel = "B:"; break;
            default:
                Debug.Log("unrecognized value " + colorComp);
                break;
        }
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
        label.text = defaultLabel + " " + Math.Round(slider.value, 2, MidpointRounding.AwayFromZero).ToString();
        lastValue = slider.value;
    }

    // set the slider's values to the newly selected object's values
    public void resetData()
    {
        float value = ObjectManager.GetCurPrimObjectValue(colorComp);
        slider.value = value;
        lastValue = value;
        UpdateLabel();
    }
}

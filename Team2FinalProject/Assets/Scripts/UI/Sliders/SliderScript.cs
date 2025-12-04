using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SliderScript : MonoBehaviour
{
    public TMP_Text label;
    protected string prefix;
    public Slider slider;
    protected float lastValue;
    protected bool setUp = false;

    void Start()
    {
        Debug.Assert(slider != null);

        
        subSetUp();
        SetPrefix();
        UpdateLabel();
        ObjectManager.curObjectChanged.AddListener(resetData);
        
    }

    protected virtual void subSetUp() {; }

    protected void Update()
    {
        if (!setUp)
        {
            resetData();
            setUp = true;
        }
    }

    protected abstract void SetPrefix();


    public abstract void SliderMoved(float value);

    // set the slider's label to the value
    protected void UpdateLabel()
    {
        if (Input.GetKey(KeyCode.LeftArrow)  || Input.GetKey(KeyCode.RightArrow) )
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
    public abstract void resetData();
}

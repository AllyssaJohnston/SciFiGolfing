using UnityEngine;

[System.Serializable]
public class SliderMode
{
    public EModifierType type;
    public float min;
    public float max;
}

public class SliderManager : MonoBehaviour
{
    private static SliderManager instance;
    private static EModifierType curSliderMode = EModifierType.ROTATION;

    [SerializeField] RGBSliderScript[] rgbSliders = new RGBSliderScript[3];
    [SerializeField] XYZSliderScript[] xyzSliders = new XYZSliderScript[3];
    [SerializeField] XYZSliderScript[] lookAtSliders = new XYZSliderScript[3];

    void Awake()
    {
        if (instance != null && instance != this)
        {
            //can't have multiples of this class
            Destroy(this);
            return;
        }
        instance = this;

        UpdateModesForSliders();
        
    }

    //user has moved the slider, update its label
    //update the scene node's attributes
    public static void SceneNodeUpdateLabel()
    {
        GameObject curSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (curSelected != null && curSelected.GetComponent<XYZSliderScript>() != null)
        {
            XYZSliderScript sliderScript = curSelected.GetComponent<XYZSliderScript>();
            sliderScript.UpdateLabel();
            ObjectManager.SetCurObjectValue(curSliderMode, sliderScript.axis, (float)sliderScript.slider.value);
        }
    }

    //user has moved the slider, update its label
    //update the prim object's attributes
    public static void RGBUpdateLabel()
    {
        GameObject curSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (curSelected != null && curSelected.GetComponent<RGBSliderScript>() != null)
        {
            RGBSliderScript sliderScript = curSelected.GetComponent<RGBSliderScript>();
            sliderScript.UpdateLabel();
            ObjectManager.SetCurPrimObjectValue(sliderScript.colorComp, (float)sliderScript.slider.value);
        }
    }

    //user has moved the slider, update its label
    //update the camera's attributes
    public static void CameraUpdateLabel()
    {
        GameObject curSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (curSelected != null && curSelected.GetComponent<XYZSliderScript>() != null)
        {
            XYZSliderScript sliderScript = curSelected.GetComponent<XYZSliderScript>();
            sliderScript.UpdateLabel();
            CameraMovement.UpdateLookAt(sliderScript.axis, (float)sliderScript.slider.value);
        }
    }

    //user has selected a new slider mode, update the values of the sliders accordingly
    private static void UpdateModesForSliders()
    {
        for (int i = 0; i < instance.xyzSliders.Length; i++)
        {
            instance.xyzSliders[i].updateSliderMode(curSliderMode);
            instance.lookAtSliders[i].updateSliderMode(0);
        }
    }

    //change the cur slider mode
    public static void UpdateSliderMode(EModifierType mode)
    {
        curSliderMode = mode;
        UpdateModesForSliders();
    }

    public static EModifierType getCurSliderMode() { return curSliderMode; }


    public void Reset()
    {
        for (int i = 0; i < instance.xyzSliders.Length; i++)
        {
            instance.xyzSliders[i].resetData();
            instance.lookAtSliders[i].resetData();
            instance.rgbSliders[i].resetData();
        }
    }
}

using UnityEngine;

[System.Serializable]

public class SliderManager : MonoBehaviour
{
    private static SliderManager instance;

    [SerializeField] XYZSliderScript[] xyzSliders = new XYZSliderScript[3]; //rotation sliders
    [SerializeField] XYZSliderScript[] lookAtSliders = new XYZSliderScript[3]; //camera sliders
     
    void Awake()
    {
        if (instance != null && instance != this)
        {
            //can't have multiples of this class
            Destroy(this);
            return;
        }
        instance = this;
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
            ObjectManager.SetCurObjectValue(sliderScript.axis, (float)sliderScript.slider.value);
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


    public void Reset()
    {
        for (int i = 0; i < instance.xyzSliders.Length; i++)
        {
            instance.xyzSliders[i].resetData();
            instance.lookAtSliders[i].resetData();
        }
    }
}

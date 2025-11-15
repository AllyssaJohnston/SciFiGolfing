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

    
    public void Reset()
    {
        for (int i = 0; i < instance.xyzSliders.Length; i++)
        {
            instance.xyzSliders[i].resetData();
            instance.lookAtSliders[i].resetData();
        }
    }

    public static void ResetCameraSliders()
    {
        for (int i = 0; i < instance.lookAtSliders.Length; i++)
        {
            instance.lookAtSliders[i].resetData();
        }
    }
}

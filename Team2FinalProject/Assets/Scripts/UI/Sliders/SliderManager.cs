using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]

public class SliderManager : MonoBehaviour
{
    private static SliderManager instance;

    [SerializeField] XYZSliderScript[] nodeSliders = new XYZSliderScript[3]; //rotation sliders
    [SerializeField] XYZSliderScript[] lookAtSliders = new XYZSliderScript[3]; //camera sliders
    [SerializeField] List<AttributeSliderScript> swingSliders = new List< AttributeSliderScript>(); // golf sliders
    [SerializeField] XYZSliderScript[] lightingSliders = new XYZSliderScript[6]; //rotation sliders
    [SerializeField] XYZSliderScript[] holeSliders = new XYZSliderScript[2]; //rotation sliders


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

    private void Start()
    {
        ObjectManager.curHoleChanged.AddListener(ResetHoleSliders);
    }

    public void Reset()
    {
        for (int i = 0; i < instance.nodeSliders.Length; i++)
        {
            instance.nodeSliders[i].resetData();
            instance.lookAtSliders[i].resetData();
            instance.lightingSliders[i].resetData();
            instance.lightingSliders[i + 3].resetData();
            
        }
        for (int i = 0; i < instance.holeSliders.Length; i++)
        {
            instance.holeSliders[i].resetData();
        }

        ResetAttributeSliders();
    }

    public static void ResetNodeSliders()
    {
        for (int i = 0; i < instance.nodeSliders.Length; i++)
        {
            instance.nodeSliders[i].resetData();
        }
    }

    public static void ResetCameraSliders()
    {
        for (int i = 0; i < instance.lookAtSliders.Length; i++)
        {
            instance.lookAtSliders[i].resetData();
        }
    }

    public static void ResetAttributeSliders()
    {
        for (int i = 0; i < instance.swingSliders.Count; i++)
        {
            instance.swingSliders[i].resetData();
        }
    }

    public static void ResetHoleSliders()
    {
        for (int i = 0; i < instance.holeSliders.Length; i++)
        {
            instance.holeSliders[i].resetData();
        }
    }

}

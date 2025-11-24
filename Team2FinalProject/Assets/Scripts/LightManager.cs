using System.Collections.Generic;
using UnityEngine;


public class LightManager : MonoBehaviour
{
    private static LightManager instance;

    public Transform diffuseLightTrans;

    public Transform PointLightTemplate;
    private Transform[] PointLightPosition = new Transform[30]; // 0 is the main point light 
    [SerializeField] float PointNear = 5.0f;
    [SerializeField] float PointFar = 10.0f;
    [SerializeField] Color PointLightColor = Color.white;

    [SerializeField] GameObject n, f;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        GameManager.lightModeChanged.AddListener(changeLighting);
        setUpDiffuseLight();
        setUpPointLightTemplate();
        changeLighting(GameManager.GetLightMode());
    }

    public void Reset()
    {
        changeLighting(GameManager.GetLightMode());
    }

    void setUpDiffuseLight()
    {
        Shader.SetGlobalFloat("minDiffuse", .6f);
        Shader.SetGlobalFloat("noDiffuse", .2f);
    }

    void setUpPointLightTemplate()
    {
        PointLightTemplate.gameObject.GetComponent<Renderer>().material.color = PointLightColor;

        Color c = PointLightColor;
        c.a = 0.2f;
        n.transform.localPosition = PointLightTemplate.localPosition;
        n.transform.localScale = new Vector3(2 * PointNear, 2 * PointNear, 2 * PointNear);
        n.GetComponent<Renderer>().material.color = c;

        c.a = 0.1f;
        f.transform.localPosition = PointLightTemplate.localPosition;
        f.transform.localScale = new Vector3(2 * PointFar, 2 * PointFar, 2 * PointFar);
        f.GetComponent<Renderer>().material.color = c;
    }
    
    void changeLighting() { changeLighting(GameManager.GetLightMode()); }

    void changeLighting(ELightMode lightMode)
    {
        int lightInt = (int)lightMode;

        bool useDiffuse = lightInt == (int)ELightMode.DIFFUSE_AND_POINT || lightInt == (int)ELightMode.DIFFUSE;
       

        bool usePoint = lightInt == (int)ELightMode.DIFFUSE_AND_POINT || lightInt == (int)ELightMode.POINT;
        if (usePoint)
        {
            n.transform.localPosition = PointLightTemplate.localPosition;
            f.transform.localPosition = PointLightTemplate.localPosition;
            //SliderManager.ResetPointLightSliders();
        }
        //SliderManager.ResetDiffuseLightSliders();
        //SliderManager.ResetPointLightSliders();

        UpdateShader(useDiffuse, usePoint);
    }

    public static void UpdateSingleDiffuseLightPosition(EAxis axis, float value)
    {
        switch (axis)
        {
            case EAxis.X: instance.diffuseLightTrans.position = new Vector3(value, instance.diffuseLightTrans.position.y,   instance.diffuseLightTrans.position.z); break;
            case EAxis.Y: instance.diffuseLightTrans.position = new Vector3(instance.diffuseLightTrans.position.x,          value, instance.diffuseLightTrans.position.z); break;
            case EAxis.Z: instance.diffuseLightTrans.position = new Vector3(instance.diffuseLightTrans.position.x,          instance.diffuseLightTrans.position.y, value); break;
            default: Debug.Log("unrecognized axis " + axis); break;
        }
        instance.n.transform.localPosition = instance.diffuseLightTrans.localPosition;
        instance.f.transform.localPosition = instance.diffuseLightTrans.localPosition;
        instance.UpdateSingleDiffuseShader();
    }

    //public static void UpdatePointLightPosition(EAxis axis, float value)
    //{
    //    switch (axis)
    //    {
    //        case EAxis.X: instance.PointLightPosition[0].position = new Vector3(value,                                      instance.PointLightPosition[0].position.y,  instance.PointLightPosition[0].position.z); break;
    //        case EAxis.Y: instance.PointLightPosition[0].position = new Vector3(instance.PointLightPosition[0].position.x,  value,                                      instance.PointLightPosition[0].position.z); break;
    //        case EAxis.Z: instance.PointLightPosition[0].position = new Vector3(instance.PointLightPosition[0].position.x,  instance.PointLightPosition[0].position.y,  value); break;
    //        default: Debug.Log("unrecognized axis " + axis); break;
    //    }
    //    instance.n.transform.localPosition = instance.PointLightPosition[0].localPosition;
    //    instance.f.transform.localPosition = instance.PointLightPosition[0].localPosition;
    //    instance.UpdatePointLightShader();
    //}

    public static Vector3 GetSingleDiffuseLightPosition() { return instance.diffuseLightTrans.position; }

    //public static Vector3 GetPointLightPosition() { return instance.PointLightPosition[0].position; }

    void UpdateShader(bool useSingleDiffuse, bool usePoint)
    {
        UpdateSingleDiffuseShader();
        UpdatePointLightShader();

        Shader.SetGlobalInteger("UseDiffuseLight", useSingleDiffuse ? 1 : 0);
        Shader.SetGlobalInteger("UsePointLight", usePoint ? 1 : 0);
    }

    void UpdateSingleDiffuseShader()
    {
        Shader.SetGlobalVector("LightPosition", diffuseLightTrans.localPosition);
    }

    void UpdatePointLightShader()
    {
        List<Vector4> lightPosition = new List<Vector4>();
        for (int i = 0; i < PointLightPosition.Length; i++)
        {
            if (PointLightPosition[i] == null) { continue; }
            lightPosition.Add(PointLightPosition[i].localPosition);
        }
        if (lightPosition.Count == 0)
        {
            lightPosition.Add(Vector4.zero);
        }
        Shader.SetGlobalVectorArray("PointLightPosition", lightPosition);
        Shader.SetGlobalColor("LightColor", PointLightColor);
        Shader.SetGlobalFloat("LightNear", PointNear);
        Shader.SetGlobalFloat("LightFar", PointFar);
       
    }
}

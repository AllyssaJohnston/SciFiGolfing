using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using UnityEngine;


public class LightManager : MonoBehaviour
{
    private static LightManager instance;

    public Transform diffuseLightTrans;

    public Transform pointLightTrans;
    private List<Vector4> PointLightPos = new List<Vector4>();  
    [SerializeField] float PointNear = 5.0f;
    [SerializeField] float PointFar = 10.0f;
    [SerializeField] Color PointLightColor = Color.white;


    private bool usePoint = false;
    private bool useSkyPoint = false;
    private bool useDiffuse = true;


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
        setUpPointLight();
        changeLighting(GameManager.GetLightMode());
    }

    private void Update() { UpdatePointLightShader(); }

    public void Reset() { changeLighting(GameManager.GetLightMode()); }

    void setUpDiffuseLight()
    {
        Shader.SetGlobalFloat("minDiffuse", .4f);
        Shader.SetGlobalFloat("noDiffuse", .2f);
    }

    void setUpPointLight()
    {
        Shader.SetGlobalColor("LightColor", instance.PointLightColor);
        Shader.SetGlobalFloat("LightNear", instance.PointNear);
        Shader.SetGlobalFloat("LightFar", instance.PointFar);
        Shader.SetGlobalFloat("maxPoint", .3f);
    }

    public static void changeLighting() { changeLighting(GameManager.GetLightMode()); }

    public static void changeLighting(ELightMode lightMode)
    {
        int lightInt = (int)lightMode;

        instance.useDiffuse = lightInt == (int)ELightMode.DIFFUSE_AND_POINT || lightInt == (int)ELightMode.DIFFUSE;
        instance.usePoint = lightInt == (int)ELightMode.DIFFUSE_AND_POINT || lightInt == (int)ELightMode.POINT;

        UpdateShader();
    }

    public static void setUseSkyLight(bool useSkyLight)
    {
        instance.useSkyPoint = useSkyLight;

    }

    //public static void UpdateSingleDiffuseLightPosition(EAxis axis, float value)
    //{
    //    switch (axis)
    //    {
    //        case EAxis.X: instance.diffuseLightTrans.position = new Vector3(value, instance.diffuseLightTrans.position.y,   instance.diffuseLightTrans.position.z); break;
    //        case EAxis.Y: instance.diffuseLightTrans.position = new Vector3(instance.diffuseLightTrans.position.x,          value, instance.diffuseLightTrans.position.z); break;
    //        case EAxis.Z: instance.diffuseLightTrans.position = new Vector3(instance.diffuseLightTrans.position.x,          instance.diffuseLightTrans.position.y, value); break;
    //        default: Debug.Log("unrecognized axis " + axis); break;
    //    }
    //    UpdateSingleDiffuseShader();
    //}

    //public static Vector3 GetSingleDiffuseLightPosition() { return instance.diffuseLightTrans.position; }

    public static void GetPointLightPositions()
    {
        instance.PointLightPos.Clear();
        GolfBallManager.getGlowingGolfBallsPos(ref instance.PointLightPos);
        GolfBallManager.getRayGolfBallsPos(ref instance.PointLightPos);


        if (instance.useSkyPoint)
        {
            instance.PointLightPos.Add(instance.pointLightTrans.position);
        }
        for (int i = instance.PointLightPos.Count; i < 30; i++) // give the gpu 30 items
        {
            instance.PointLightPos.Add(new Vector4(1000, 0, 0, 0));
        }

        Shader.SetGlobalInteger("UsePointLight", instance.usePoint ? 1 : 0);
    }

    static void UpdateShader()
    {
        UpdateSingleDiffuseShader();
        UpdatePointLightShader();

        Shader.SetGlobalInteger("UseDiffuseLight", instance.useDiffuse ? 1 : 0);
        Shader.SetGlobalInteger("UsePointLight", instance.usePoint ? 1 : 0);
    }

    static void UpdateSingleDiffuseShader() { Shader.SetGlobalVector("DiffusePosition", instance.diffuseLightTrans.localPosition); }

    static void UpdatePointLightShader()
    {
        GetPointLightPositions();
        Shader.SetGlobalVectorArray("PointLightPosition", instance.PointLightPos);
    }
}

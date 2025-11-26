using System.Collections.Generic;
using UnityEngine;


public class LightManager : MonoBehaviour
{
    private static LightManager instance;

    private Vector3 startingDiffuseLightPos;
    [SerializeField] Vector3 diffuseLightPos;

    private Vector3 startingSkyLightPos;
    [SerializeField] Vector3 skyLightPos;
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
        startingDiffuseLightPos = diffuseLightPos;
        startingSkyLightPos = skyLightPos;
        setUpDiffuseLight();
        setUpPointLight();
        changeLighting(GameManager.GetLightMode());
    }

    private void Update() { UpdatePointLightShader(); }

    public void Reset() 
    {
        diffuseLightPos = startingDiffuseLightPos;
        skyLightPos = startingSkyLightPos;
        changeLighting(GameManager.GetLightMode()); 
    }

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

    public static void setUseSkyLight(bool useSkyLight) { instance.useSkyPoint = useSkyLight; }

    public static void UpdateDiffuseLightPosition(EAxis axis, float value)
    {
        switch (axis)
        {
            case EAxis.X: instance.diffuseLightPos = new Vector3(value, instance.diffuseLightPos.y, instance.diffuseLightPos.z); break;
            case EAxis.Y: instance.diffuseLightPos = new Vector3(instance.diffuseLightPos.x, value, instance.diffuseLightPos.z); break;
            case EAxis.Z: instance.diffuseLightPos = new Vector3(instance.diffuseLightPos.x, instance.diffuseLightPos.y, value); break;
            default: Debug.Log("unrecognized axis " + axis); break;
        }
        UpdateDiffuseShader();
    }

    public static void UpdatePointLightPosition(EAxis axis, float value)
    {
        switch (axis)
        {
            case EAxis.X: instance.skyLightPos = new Vector3(value, instance.skyLightPos.y, instance.skyLightPos.z); break;
            case EAxis.Y: instance.skyLightPos = new Vector3(instance.skyLightPos.x, value, instance.skyLightPos.z); break;
            case EAxis.Z: instance.skyLightPos = new Vector3(instance.skyLightPos.x, instance.skyLightPos.y, value); break;
            default: Debug.Log("unrecognized axis " + axis); break;
        }
        UpdatePointLightShader();
    }

    public static Vector3 GetDiffuseLightPosition() { return instance.diffuseLightPos; }

    public static Vector3 GetPointLightPosition() { return instance.skyLightPos; }

    public static void GetPointLightPositions()
    {
        instance.PointLightPos.Clear();
        if (instance.useSkyPoint)
        {
            instance.PointLightPos.Add(instance.skyLightPos);
        }
        GolfBallManager.getGlowingGolfBallsPos(ref instance.PointLightPos);
        GolfBallManager.getRayGolfBallsPos(ref instance.PointLightPos);
        for (int i = instance.PointLightPos.Count; i < 30; i++) // give the gpu 30 items
        {
            instance.PointLightPos.Add(new Vector4(1000, 0, 0, 0));
        }

        Shader.SetGlobalInteger("UsePointLight", instance.usePoint ? 1 : 0);
    }

    static void UpdateShader()
    {
        UpdateDiffuseShader();
        UpdatePointLightShader();

        Shader.SetGlobalInteger("UseDiffuseLight", instance.useDiffuse ? 1 : 0);
        Shader.SetGlobalInteger("UsePointLight", instance.usePoint ? 1 : 0);
    }

    static void UpdateDiffuseShader() { Shader.SetGlobalVector("DiffusePosition", instance.diffuseLightPos); }

    static void UpdatePointLightShader()
    {
        GetPointLightPositions();
        Shader.SetGlobalVectorArray("PointLightPosition", instance.PointLightPos);
    }
}

using System.Collections.Generic;
using UnityEngine;




public class LightManager : MonoBehaviour
{
    private static LightManager instance;

    public Transform LightPosition1;
    
    public Vector3 diffuselightPos;
    public Vector3 pointLightPos;

    public Transform PointLightPosition;
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
        //GameManager.gameModeChanged.AddListener(changeLighting);
        setUpPointLight();
        changeLighting(GameManager.GetLightMode());
        //InputManager.resetOp.AddListener(Reset);

    }

    public void Reset()
    {
        changeLighting(GameManager.GetLightMode());
    }

    void setUpPointLight()
    {
        PointLightPosition.gameObject.GetComponent<Renderer>().material.color = PointLightColor;

        Color c = PointLightColor;
        c.a = 0.2f;
        n.transform.localPosition = PointLightPosition.localPosition;
        n.transform.localScale = new Vector3(2 * PointNear, 2 * PointNear, 2 * PointNear);
        n.GetComponent<Renderer>().material.color = c;

        c.a = 0.1f;
        f.transform.localPosition = PointLightPosition.localPosition;
        f.transform.localScale = new Vector3(2 * PointFar, 2 * PointFar, 2 * PointFar);
        f.GetComponent<Renderer>().material.color = c;
    }
    
    void changeLighting() { changeLighting(GameManager.GetLightMode()); }

    void changeLighting(ELightMode lightMode)
    {
        int lightInt = (int)lightMode;

        bool useDiffuse = lightInt == (int)ELightMode.DIFFUSE_AND_POINT || lightInt == (int)ELightMode.DIFFUSE;
        if (useDiffuse)
        {
            LightPosition1.position = diffuselightPos;
        }
       

        bool usePoint = lightInt == (int)ELightMode.DIFFUSE_AND_POINT || lightInt == (int)ELightMode.POINT;
        if (usePoint)
        {
            PointLightPosition.position = pointLightPos;
            n.transform.localPosition = PointLightPosition.localPosition;
            f.transform.localPosition = PointLightPosition.localPosition;
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
            case EAxis.X: instance.LightPosition1.position = new Vector3(value, instance.LightPosition1.position.y, instance.LightPosition1.position.z); break;
            case EAxis.Y: instance.LightPosition1.position = new Vector3(instance.LightPosition1.position.x, value, instance.LightPosition1.position.z); break;
            case EAxis.Z: instance.LightPosition1.position = new Vector3(instance.LightPosition1.position.x, instance.LightPosition1.position.y, value); break;
            default: Debug.Log("unrecognized axis " + axis); break;
        }
        instance.n.transform.localPosition = instance.LightPosition1.localPosition;
        instance.f.transform.localPosition = instance.LightPosition1.localPosition;
        instance.UpdateSingleDiffuseShader();
    }

    public static void UpdatePointLightPosition(EAxis axis, float value)
    {
        switch (axis)
        {
            case EAxis.X: instance.PointLightPosition.position = new Vector3(value,                                  instance.PointLightPosition.position.y, instance.PointLightPosition.position.z); break;
            case EAxis.Y: instance.PointLightPosition.position = new Vector3(instance.PointLightPosition.position.x, value,                                  instance.PointLightPosition.position.z); break;
            case EAxis.Z: instance.PointLightPosition.position = new Vector3(instance.PointLightPosition.position.x, instance.PointLightPosition.position.y, value); break;
            default: Debug.Log("unrecognized axis " + axis); break;
        }
        instance.n.transform.localPosition = instance.PointLightPosition.localPosition;
        instance.f.transform.localPosition = instance.PointLightPosition.localPosition;
        instance.UpdatePointLightShader();
    }

    public static Vector3 GetSingleDiffuseLightPosition() { return instance.LightPosition1.position; }

    public static Vector3 GetPointLightPosition() { return instance.PointLightPosition.position; }

    void UpdateShader(bool useSingleDiffuse, bool usePoint)
    {
        UpdateSingleDiffuseShader();
        UpdatePointLightShader();

        Shader.SetGlobalInteger("UseDiffuseLight", useSingleDiffuse ? 1 : 0);
        Shader.SetGlobalInteger("UsePointLight", usePoint ? 1 : 0);
    }
    void UpdateSingleDiffuseShader()
    {
        Shader.SetGlobalVector("LightPosition", LightPosition1.localPosition);
    }

    void UpdatePointLightShader()
    {
        Shader.SetGlobalVector("PointLightPosition", PointLightPosition.localPosition);
        Shader.SetGlobalColor("LightColor", PointLightColor);
        Shader.SetGlobalFloat("LightNear", PointNear);
        Shader.SetGlobalFloat("LightFar", PointFar);
       
    }
}

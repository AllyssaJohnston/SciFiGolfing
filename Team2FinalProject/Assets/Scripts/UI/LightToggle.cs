using UnityEngine;
using UnityEngine.UI;

public enum ELightToggle
{
    DIFFUSE = 0,
    POINT = 1
}

public class LightToggle : MonoBehaviour
{
    private Toggle toggle;
    private bool starting;
    [SerializeField] ELightToggle toggleType;

    private void Start()
    {
        toggle = gameObject.GetComponent<Toggle>();
        toggle.isOn = GameManager.GetLightMode() == ELightMode.DIFFUSE || GameManager.GetLightMode() == ELightMode.DIFFUSE_AND_POINT;
        starting = toggle.isOn;
        toggle.onValueChanged.AddListener(OnClick);
        LightManager.setUseSkyLight(toggle.isOn);
        ObjectManager.resetWorld.AddListener(Reset);
    }

    public void OnClick(bool value)
    {
        if (toggleType == ELightToggle.DIFFUSE)
        {
            GameManager.SetDiffuse(value);
        }
        else
        {
            GameManager.SetPoint(value);
            LightManager.setUseSkyLight(value);
        }
            
    }

    public void Reset()
    {
        toggle.isOn = starting;
        OnClick(starting);
    }
}

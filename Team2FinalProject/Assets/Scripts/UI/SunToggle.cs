using UnityEngine;
using UnityEngine.UI;

public class SunToggle : MonoBehaviour
{
    private Toggle toggle;

    private void Start()
    {
        toggle = gameObject.GetComponent<Toggle>();
        toggle.isOn = GameManager.GetLightMode() == ELightMode.DIFFUSE || GameManager.GetLightMode() == ELightMode.DIFFUSE_AND_POINT;
        toggle.onValueChanged.AddListener(OnClick);
    }

    public void OnClick(bool value)
    {
        GameManager.SetDiffuse(value);
    }
}

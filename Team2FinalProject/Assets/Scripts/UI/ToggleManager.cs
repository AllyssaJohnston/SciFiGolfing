using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    private static ToggleManager instance;
    [SerializeField] List<Toggle> modifierToggles = new List<Toggle>();
    [SerializeField] Toggle axisToggle;

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

    // click on the rotation or scale toggles
    public static void ModifierButtonClick(bool value)
    {
        GameObject curSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (value && curSelected != null && curSelected.GetComponent<ModifierToggleSwitch>() != null)
        {
            SliderManager.UpdateSliderMode(curSelected.GetComponent<ModifierToggleSwitch>().modifierType);
        }
    }

    // click on the axis toggle
    public static void AxisClick(bool value) {  AxesManager.SetAxisVisibility(value); }

    public static void ResetData()
    {
        instance.modifierToggles[0].isOn = true;
        for (int i = 1; i < instance.modifierToggles.Count; i++)
        {
            instance.modifierToggles[i].isOn = false;
        }
        SliderManager.UpdateSliderMode(instance.modifierToggles[0].gameObject.GetComponent<ModifierToggleSwitch>().modifierType);
        instance.axisToggle.isOn = true;
    }
}

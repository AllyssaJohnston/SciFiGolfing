using UnityEngine;

public enum EModifierType
{
    POSITION = 0,
    ROTATION = 1,
    SCALE = 2
}

public class ModifierToggleSwitch : MonoBehaviour
{
    public EModifierType modifierType;
}

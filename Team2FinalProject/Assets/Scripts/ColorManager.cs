using UnityEngine;
using UnityEngine.Events;


public enum EColor
{
    ESKIN = 0,
    EMAIN,
    ESECONDARY,
    ESPHERE,
    ECLUB
}

[ExecuteInEditMode]
public class ColorManager : MonoBehaviour
{
    private static ColorManager instance;
    [SerializeField] Color Skin = Color.yellow;
    [SerializeField] Color Main = Color.cyan;
    [SerializeField] Color Secondary = Color.white;
    [SerializeField] Color Sphere = Color.yellow;
    [SerializeField] Color Club = Color.lightCyan;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public static Color GetColor(EColor color)
    {
        switch (color)
        {
            case EColor.ESKIN:
                return instance.Skin;
            case EColor.EMAIN:
                return instance.Main;
            case EColor.ESECONDARY:
                return instance.Secondary;
            case EColor.ESPHERE:
                return instance.Sphere;
            case EColor.ECLUB:
                return instance.Club;
            default:
                Debug.Log("invalid color " + color);
                return Color.blue;
        }
    }

}
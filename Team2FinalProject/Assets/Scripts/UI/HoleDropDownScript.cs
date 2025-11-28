using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoleDropDownScript : MonoBehaviour
{
    private static HoleDropDownScript instance;
    public TMP_Dropdown menu = null;

    private static List<TMP_Dropdown.OptionData> selectMenuOptions = new List<TMP_Dropdown.OptionData>();
    public List<GameObject> holes = new List<GameObject>();

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        Debug.Assert(menu != null);
    }

    private void Start()
    {
        Debug.Assert(menu != null);

        GetOptions();
        menu.onValueChanged.AddListener(SelectionChange);
        ObjectManager.SetCurHoleObject(holes[0]);
    }

    private static void GetOptions()
    {
        instance.menu.ClearOptions();
        for (int i = 0; i < instance.holes.Count; i++)
        {
            TMP_Dropdown.OptionData d = new TMP_Dropdown.OptionData(instance.holes[i].name);
            selectMenuOptions.Add(d);
        }
        instance.menu.AddOptions(selectMenuOptions); // adds all options
    }

    // called when drop down menu value changes
    public static void SelectionChange(int index) { ObjectManager.SetCurHoleObject(instance.holes[index]); }

    // called from other sources
    public static void SelectionChange(GameObject hole)
    {
        int i = instance.holes.IndexOf(hole);
        instance.menu.value = i;
        SelectionChange(i);
    }

    public static void ResetData()
    {
        instance.menu.value = 0;
        ObjectManager.SetCurHoleObject(instance.holes[0]);
    }
}

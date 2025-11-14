using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


public class PrimitiveDropDownControl : MonoBehaviour
{
    private static PrimitiveDropDownControl instance;
    public TMP_Dropdown menu = null;

    private const string kChildSpace = " ";
    private static List<TMP_Dropdown.OptionData> selectMenuOptions = new List<TMP_Dropdown.OptionData>();
    private static List<NodePrimitive> mPrimitives = new List<NodePrimitive>();


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
        GetOptions();
        menu.onValueChanged.AddListener(SelectionChange);
        ObjectManager.SetCurPrimObject(World.GetRoot().PrimitiveList[0]);
    }

    private static void GetOptions()
    {
        instance.menu.ClearOptions();
        selectMenuOptions.Add(new TMP_Dropdown.OptionData(World.GetRoot().PrimitiveList[0].name));
        mPrimitives.Add(World.GetRoot().PrimitiveList[0]);
        GetChildrenNames("", World.GetRoot());
        instance.menu.AddOptions(selectMenuOptions); // adds all options
    }

    private static void GetChildrenNames(string blanks, SceneNode node)
    {
        string space = blanks + kChildSpace;
        List<SceneNode> children = node.ChildrenList;
        for (int i = children.Count - 1; i >= 0; i--)
        {
            for (int j = children[i].PrimitiveList.Count - 1; j >= 0; j--)
            {
                TMP_Dropdown.OptionData d = new TMP_Dropdown.OptionData(space + children[i].PrimitiveList[j].name);
                selectMenuOptions.Add(d);
                mPrimitives.Add(children[i].PrimitiveList[j]);
                GetChildrenNames(space, children[i]);
            }
        }
    }

    public static void SelectionChange(int index) { ObjectManager.SetCurPrimObject(mPrimitives[index]); }

    public static void ResetData() { instance.menu.value = 0; }
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SceneNodeDropDownControl : MonoBehaviour
{
    private static SceneNodeDropDownControl instance;
    public TMP_Dropdown menu = null;

    private const string kChildSpace = " ";
    private static List<TMP_Dropdown.OptionData> selectMenuOptions = new List<TMP_Dropdown.OptionData>();
    private static List<SceneNode> listSceneNodes = new List<SceneNode>();

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
        ObjectManager.SetCurObject(listSceneNodes[0]);
    }

    private static void GetOptions()
    {
        instance.menu.ClearOptions();
        if (World.GetRoot().editable)
        {
            selectMenuOptions.Add(new TMP_Dropdown.OptionData(World.GetRoot().transform.name));
            listSceneNodes.Add(World.GetRoot());
        }
        GetChildrenNames("", World.GetRoot());
        instance.menu.AddOptions(selectMenuOptions); // adds all options
    }

    private static void GetChildrenNames(string blanks, SceneNode node)
    {
        string space = blanks + kChildSpace;
        List<SceneNode> children = node.ChildrenList;
        for (int i = children.Count - 1; i >= 0; i--)
        {
            if (node.ChildrenList[i].editable)
            {
                TMP_Dropdown.OptionData d = new TMP_Dropdown.OptionData(space + children[i].transform.name);
                selectMenuOptions.Add(d);
                listSceneNodes.Add(children[i]);
            }
            GetChildrenNames(space, children[i]);
        }
    }

    // called when drop down menu value changes
    public static void SelectionChange(int index) { ObjectManager.SetCurObject(listSceneNodes[index]); }

    public static void ResetData() { instance.menu.value = 0; }
}

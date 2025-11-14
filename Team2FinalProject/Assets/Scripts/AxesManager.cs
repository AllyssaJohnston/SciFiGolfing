using UnityEngine;

public class AxesManager : MonoBehaviour
{
    private static AxesManager instance;
    public GameObject axes;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        ObjectManager.curObjectChanged.AddListener(SetAxes);
        ObjectManager.curSceneObjectValuesChanged.AddListener(SetAxes);
    }

    // set the axes position
    private static void SetAxes()
    {
        SceneNode node = ObjectManager.GetCurObject();
        Vector4 fourthColumn = node.getXForm().GetColumn(3);
        Vector3 position = new Vector3(fourthColumn.x, fourthColumn.y, fourthColumn.z);
        instance.axes.transform.position = position;
        instance.axes.transform.localRotation = node.transform.rotation;
    }

    // change the axes visibility
    public static void SetAxisVisibility(bool visible) { instance.axes.SetActive(visible); }
}

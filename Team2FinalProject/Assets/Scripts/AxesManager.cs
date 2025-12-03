using UnityEngine;

public class AxesManager : MonoBehaviour
{
    private static AxesManager instance;
    public GameObject axes;
    private bool tickYet = false;

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
        GameManager.gameModeChanged.AddListener(GameModeChanged);
    }
    public void Update()
    {
        if (!tickYet)
        {
            SetAxes();
            tickYet = true;
        }
        
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

    public static void GameModeChanged() { SetAxisVisibility(GameManager.GetGameMode() == EGameMode.SETUP); }

    // change the axes visibility
    public static void SetAxisVisibility(bool visible) { instance.axes.SetActive(visible); }

}

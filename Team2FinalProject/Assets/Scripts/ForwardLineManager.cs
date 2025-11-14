using UnityEngine;

public class ForwardLineManager : MonoBehaviour
{
    private static ForwardLineManager instance;
    public GameObject forwardLine;
    private static Quaternion startingRotation;
    bool setUp = false;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        ObjectManager.curObjectChanged.AddListener(SetForwardLine);
        ObjectManager.curSceneObjectValuesChanged.AddListener(SetForwardLine);
        startingRotation = forwardLine.transform.rotation;
    }

    public void Update()
    {
        if (!setUp)
        {
            SetForwardLine();
            setUp = true;
        }
    }

    // set the line position
    private static void SetForwardLine()
    {
        SceneNode node = ObjectManager.GetCurObject();
        
        instance.forwardLine.transform.rotation = startingRotation;
        instance.forwardLine.transform.rotation = node.transform.rotation * Quaternion.FromToRotation(instance.forwardLine.transform.forward, node.forwardDir);
        instance.forwardLine.transform.position = node.getXForm().GetPosition() - (instance.forwardLine.transform.up * instance.forwardLine.transform.localScale.y);
        if (node.gameObject.name == "BottomNode")
        {
            instance.forwardLine.transform.position += Vector3.up;
        }

    }

}

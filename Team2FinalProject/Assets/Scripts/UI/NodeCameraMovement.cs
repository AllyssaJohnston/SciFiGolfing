using UnityEngine;

public class NodeCameraMovement : MonoBehaviour
{
    private static NodeCameraMovement instance;
    private static SceneNode node;


    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectManager.curObjectChanged.AddListener(ChangeNode);
        node = World.GetRoot();
        UpdateMovement();
    }

    // Update is called once per frame
    void Update() { UpdateMovement(); }

    public void ChangeNode() {  node = ObjectManager.GetCurObject(); }

    private void UpdateMovement()
    {
        Vector4 fourthColumn = node.getXForm().GetColumn(3);
        transform.position = new Vector3(fourthColumn.x, fourthColumn.y, fourthColumn.z);
        transform.rotation = Quaternion.identity;
        transform.rotation = node.transform.rotation * Quaternion.FromToRotation(transform.forward, node.forwardDir);

        if (node.forwardDir.x < 0 || node.forwardDir.y < 0 || node.forwardDir.z < 0)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 180);
        }
            
    }
}

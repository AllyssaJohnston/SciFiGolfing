using UnityEngine;

public class NodeCameraMovement : MonoBehaviour
{
    private static NodeCameraMovement instance;
    public NodePrimitive node;
    public Vector3 dir;

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
        UpdateMovement();
    }

    // Update is called once per frame
    void Update() { UpdateMovement(); }

    private void UpdateMovement()
    {
        Vector4 fourthColumn = node.getXForm().GetColumn(3);
        transform.position = new Vector3(fourthColumn.x, fourthColumn.y, fourthColumn.z);
        transform.rotation = Quaternion.identity;
        transform.rotation = node.transform.rotation * Quaternion.FromToRotation(transform.forward, dir);

        if (dir.x < 0 || dir.y < 0 || dir.z < 0)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 180);
        }
            
    }
}

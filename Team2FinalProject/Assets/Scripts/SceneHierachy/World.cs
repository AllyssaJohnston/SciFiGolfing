using UnityEngine;

[ExecuteInEditMode]
public class World : MonoBehaviour
{
    private static World instance;
    public SceneNode root;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Update()
    {
        Matrix4x4 i = Matrix4x4.identity;
        root.CompositeXform(ref i);
    }

    public static SceneNode GetRoot() { return instance.root; }

    public static void Reset()
    {
        Matrix4x4 i = Matrix4x4.identity;
        instance.root.ResetNode(ref i);
        ObjectManager.SetCurObject(instance.root);
    }

}

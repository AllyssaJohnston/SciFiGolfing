using UnityEngine;
using System.Collections.Generic;

public class SceneNode : MonoBehaviour 
{
    protected Matrix4x4 mCombinedParentXform;
    
    public Vector3 NodeOrigin = Vector3.zero;
    [HideInInspector] public Vector3 rotation = Vector3.zero; // for keeping track of rotation changes in UI
    private Vector3 startingPos;
    private Quaternion startingRot;
    private Vector3 startingScale;
    public Vector3 forwardDir;

    public List<NodePrimitive> PrimitiveList;
    public List<SceneNode> ChildrenList;

    public bool editable = true;
    public Vector3 editableAxes = Vector3.one;

	protected void Start () 
    { 
        mCombinedParentXform = Matrix4x4.identity;
        startingPos = transform.localPosition;
        startingRot = transform.localRotation;
        startingScale = transform.localScale;
    }

    // This must be called _BEFORE_ each draw
    public void CompositeXform(ref Matrix4x4 parentXform)
    {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        
        mCombinedParentXform = parentXform * orgT * trs;

        // propagate to all children
        foreach (SceneNode child in ChildrenList)
        {
            child.CompositeXform(ref mCombinedParentXform);
        }

        // disseminate to primitives
        foreach (NodePrimitive p in PrimitiveList)
        {
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }
    }

    public Matrix4x4 getXForm() { return mCombinedParentXform; }

    public void setXForm(Matrix4x4 m) { mCombinedParentXform = m; }

    public void ResetNode(ref Matrix4x4 parentXform)
    {
        transform.localPosition = startingPos;
        transform.localRotation = startingRot;
        transform.localScale = startingScale;
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        mCombinedParentXform = parentXform * orgT * trs;
        // propagate to all children
        foreach (SceneNode child in ChildrenList)
        {
            child.ResetNode(ref mCombinedParentXform);
        }

        foreach (NodePrimitive p in PrimitiveList)
        {
            p.ResetNode(ref mCombinedParentXform);
        }

        rotation = Vector3.zero; // for keeping track of rotation changes in UI
    }
}
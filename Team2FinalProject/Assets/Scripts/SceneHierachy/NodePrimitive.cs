using UnityEngine;



public class NodePrimitive: MonoBehaviour {
    [SerializeField] EColor color;
    private Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    private Color startingColor;
    public Vector3 Pivot;
    private Vector3 startingPos;
    private Quaternion startingRot;
    private Vector3 startingScale;
    private Material material = null;
    private bool init = false;

    protected void Start()
    {
        startingPos = transform.localPosition;
        startingRot = transform.localRotation;
        startingScale = transform.localScale;
        MyColor = ColorManager.GetColor(color);

        startingColor = MyColor;
        if (init == false || material == null) // initialize
        {
            material = GetComponent<Renderer>().material;
            init = true;
        }
    }

    public void LoadShaderMatrix(ref Matrix4x4 nodeMatrix)
    {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Matrix4x4 m = nodeMatrix * p * trs * invp;
        if (init == false || material == null ) // initialize
        {
            material = GetComponent<Renderer>().material;
            MyColor = ColorManager.GetColor(color);
            init = true;
        }
        material.SetMatrix("MyXformMat", m);
        material.SetColor("MyColor", MyColor);
    }

    public void ResetNode(ref Matrix4x4 nodeMatrix)
    {
        transform.localPosition = startingPos;
        transform.localRotation = startingRot;
        transform.localScale = startingScale;
        // reset the animation if it exists
        AnimatePrimitive ap = gameObject.GetComponent<AnimatePrimitive>();
        if (ap != null) 
        {
            ap.ResetNode();
        }

        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Matrix4x4 m = nodeMatrix * p * trs * invp;
        material.SetMatrix("MyXformMat", m);
        material.SetColor("MyColor", startingColor);
        MyColor = startingColor;
    }

    public void UpdateColor(Color color) 
    {
        MyColor = color;
        material.SetColor("MyColor", MyColor); 
    }

    public Color GetColor() { return MyColor; }
}
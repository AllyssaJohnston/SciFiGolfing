using UnityEngine;


public class NodePrimitive: MonoBehaviour {
    [SerializeField] EColor color;
    private Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;
    private Vector3 startingPos;
    private Quaternion startingRot;
    private Vector3 startingScale;
    private Material material = null;
    private Matrix4x4 m;
    [SerializeField] private GameObject colliderObj = null;
    private int nodeColliderLayer;
    private bool init = false;

    protected void Awake()
    {
        init = false;
    }

    protected void Start()
    {
        nodeColliderLayer = LayerMask.NameToLayer("NodeCollider");
        startingPos = transform.localPosition;
        startingRot = transform.localRotation;
        startingScale = transform.localScale;
        MyColor = ColorManager.GetColor(color);
        if (init == false || material == null) // initialize
        {
            setUpCollider();

            material = GetComponent<Renderer>().material;
            init = true;
        }
    }

    public void LoadShaderMatrix(ref Matrix4x4 nodeMatrix)
    {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        m = nodeMatrix * p * trs * invp;
        if (init == false || material == null ) // initialize
        {
            material = GetComponent<Renderer>().sharedMaterial;
            MyColor = ColorManager.GetColor(color);
            init = true;
        }
        material.SetMatrix("MyXformMat", m);
        moveCollider(m);
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
        material.SetColor("MyColor", MyColor);
    }

    public void UpdateColor(Color color) 
    {
        MyColor = color;
        material.SetColor("MyColor", MyColor); 
    }

    public Color GetColor() { return MyColor; }

    public Matrix4x4 getXForm() { return m; }

    private void setUpCollider()
    {   
        if (!Application.isPlaying)
        {
            return;
        }
        colliderObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        colliderObj.layer = nodeColliderLayer;
        colliderObj.transform.parent = transform;
        DestroyImmediate(colliderObj.GetComponent<Collider>());
        colliderObj.GetComponent<MeshRenderer>().enabled = false;
        Collider nodeCollider = GetComponent<Collider>();
        if (nodeCollider is SphereCollider)
        {
            colliderObj.AddComponent<SphereCollider>();
        }
        else if (nodeCollider is CapsuleCollider)
        {
            colliderObj.AddComponent<CapsuleCollider>();
        }
        else if (nodeCollider is BoxCollider)
        {
            colliderObj.AddComponent<BoxCollider>();
        }
        else
        {
            Debug.Log("unrecognized collider type");
        }
        Rigidbody rb = colliderObj.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 5.0f;

    }

    private void moveCollider(Matrix4x4 m)
    {
        if (colliderObj == null)
        {
            return;
        }
        Vector4 fourthColumn = m.GetColumn(3);
        Vector3 position = new Vector3(fourthColumn.x, fourthColumn.y, fourthColumn.z);
        colliderObj.transform.position = position;
        colliderObj.transform.localRotation = transform.rotation;
    }

}
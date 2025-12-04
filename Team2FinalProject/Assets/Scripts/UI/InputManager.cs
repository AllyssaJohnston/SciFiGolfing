using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    private bool mouseDownLastFrame;
    LayerMask nodeColliderMask;
    LayerMask holeMask;
    LayerMask groundMask;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            //can't have multiples of this class
            Destroy(this);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        string[] colliderlayers = { "NodeCollider" };
        nodeColliderMask = LayerMask.GetMask(colliderlayers);

        string [] holeLayers = { "hole", "HoleObj"};
        holeMask = LayerMask.GetMask(holeLayers);

        string[] groundLayers = { "ground" };
        groundMask = LayerMask.GetMask(groundLayers);
    }

    private void Update() { useInput(); }

    private void useInput()
    {
        bool curMouseDown = Mouse.current.leftButton.isPressed;
        if (curMouseDown && !mouseDownLastFrame)
        {
            leftMouseClick();
        }
        else if (curMouseDown && mouseDownLastFrame)
        {
            leftMouseHeld();
        }
        mouseDownLastFrame = curMouseDown;
    }

    private void useKeyBoard()
    {

        SceneNode sceneNode = ObjectManager.GetCurObject();
        if (sceneNode != null && ObjectManager.GetLastChanged() == ELastChanged.SCENE_NODE)
        {
            //if (Input.GetKey(KeyCode.W))
            //{
            //    ObjectManager.moveControllerBy(EAxis.Y, transPerSec * Time.deltaTime);
            //    SliderManager.ResetNodeSliders();
            //}

            //sceneNode.gameObject.transform.position;
            ObjectManager.curSceneObjectValuesChanged.Invoke();
            SliderManager.ResetNodeSliders();

        }
    }

    private void leftMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
        {
            // on UI, ignore
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, nodeColliderMask))
        {
            // select a node collider
            SceneNode curObject = hit.collider.gameObject.GetComponentInParent<SceneNode>();
            if (curObject.editable)
            {
                Debug.Log("hit " + curObject);
                ObjectManager.SetCurObject(curObject);
                SceneNodeDropDownControl.SelectionChange(curObject);
            }
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, holeMask) && GameManager.GetGameMode() == EGameMode.SETUP)
        {
            // select a hole
            GameObject curObject = hit.collider.gameObject.transform.parent.gameObject;
            Debug.Log("hit " + curObject);
            ObjectManager.SetCurHoleObject(curObject);
            HoleDropDownControl.SelectionChange(curObject);
        }
    }

    private void leftMouseHeld()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        
        if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
        {
            // on UI or null, ignore
            return;
        }
        // move the hole along x and y axes of the mouse. Also have sliders to control sphere's x, y, and z
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            GameObject hole = ObjectManager.GetCurHoleObject();            
            if ((hole != null) && (ObjectManager.GetLastChanged() == ELastChanged.HOLE))
            {
                Vector3 newPos = new Vector3(hit.point.x, hole.transform.position.y, hit.point.z);
                if ((hole.transform.position - newPos).magnitude < 10)
                {
                    hole.transform.position = newPos;
                    SliderManager.ResetHoleSliders();
                }
                
            }
        }
    }  
}

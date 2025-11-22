using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    private Vector2 mousePos;
    private Vector2 lastMousePos;
    private bool mouseDownLastFrame;
    LayerMask nodeColliderMask;



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
        string[] layers = { "NodeCollider" };
        nodeColliderMask = LayerMask.GetMask(layers);
    }

    private void Update()
    {
        useInput();
    }

    private void useInput()
    {
        lastMousePos = new Vector2(mousePos.x, mousePos.y);
        mousePos = Input.mousePosition;

        //if (Input.GetKey(KeyCode.R))
        //{
        //    ResetCalled();
        //    return;
        //}
       
        bool curMouseDown = Mouse.current.leftButton.isPressed;

        if (curMouseDown && !mouseDownLastFrame)
        {
            leftMouseClick();
        }
        //else if (curMouseDown && mouseDownLastFrame)
        //{
        //    leftMouseHeld();
        //}
        mouseDownLastFrame = curMouseDown;
    }


    private void leftMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (EventSystem.current.IsPointerOverGameObject() || UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
        {
            // on UI, ignore
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, nodeColliderMask))
        {
            // select a sphere
            SceneNode curObject = hit.collider.gameObject.GetComponentInParent<SceneNode>();
            if (curObject.editable)
            {
                Debug.Log("hit " + curObject);
                ObjectManager.SetCurObject(curObject);
                SceneNodeDropDownControl.SelectionChange(curObject);
            }
        }
    }
}

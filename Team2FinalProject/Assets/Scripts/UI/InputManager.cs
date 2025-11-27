using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    private Vector2 mousePos;
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
        mousePos = Input.mousePosition;
       
        bool curMouseDown = Mouse.current.leftButton.isPressed;

        if (curMouseDown && !mouseDownLastFrame)
        {
            leftMouseClick();
        }
        mouseDownLastFrame = curMouseDown;
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

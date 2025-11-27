using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour
{

    public GameObject parent;
    private RectTransform parentRect;
    public GameObject panel;
    private float panelHeight;
    public Toggle toggle;
    private float toggleHeight;

    private void Start()
    {
        toggle.onValueChanged.AddListener(SetVisibility);
        parentRect = parent.GetComponent<RectTransform>();
        panelHeight = panel.GetComponent<RectTransform>().rect.height;
        toggleHeight = toggle.GetComponent<RectTransform>().rect.height + 50;
    }

    public void SetVisibility(bool value)
    {
        parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value ? panelHeight : toggleHeight);
        panel.SetActive(value);
        parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value ? panelHeight : toggleHeight);
    }

}

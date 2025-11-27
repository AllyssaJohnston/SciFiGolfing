using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour
{
    public EGameMode whenToShow;
    public GameObject parent;
    private RectTransform parentRect;
    public GameObject panel;
    private float panelHeight;
    public Toggle toggle;
    public TMP_Text title;
    private float toggleHeight;
    private bool defaultToggleSetting = true;
    private bool setUp = false;

    private void Start()
    {
        defaultToggleSetting = toggle.isOn;
        toggle.onValueChanged.AddListener(OnClick);
        parentRect = parent.GetComponent<RectTransform>();
        panelHeight = panel.GetComponent<RectTransform>().rect.height;
        toggleHeight = toggle.GetComponent<RectTransform>().rect.height + 50;
    }

    private void Update()
    {
        if (!setUp)
        {
            SetVisibility();
            setUp = true;
        }
    }

   
    public void OnClick(bool value)
    {
        setVisibilityInternal(value);
    }

    public void SetVisibility() // setting toggle listener to this, (which modifies toggle value) will create infinite loop
    {
        bool value = GameManager.GetGameMode() == whenToShow;
        
        toggle.gameObject.SetActive(value);
        panel.gameObject.SetActive(value);
        title.gameObject.SetActive(value);
        if (value && !defaultToggleSetting)
        {
            value = false;
        }
        setVisibilityInternal(value);
    }

    public void SetVisibility(bool value)
    {
       
        toggle.gameObject.SetActive(value);
        panel.gameObject.SetActive(value);
        title.gameObject.SetActive(value);
        setVisibilityInternal(value);
    }

    private void setVisibilityInternal(bool value)
    {
        toggle.isOn = value;
        title.color = value ? Color.black : Color.white;
        panel.SetActive(value);
        parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, toggle.gameObject.activeSelf ? (value ? panelHeight : toggleHeight) : 0);
    }

}

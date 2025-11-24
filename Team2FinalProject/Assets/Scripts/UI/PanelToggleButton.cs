using UnityEngine;
using UnityEngine.UI;

public class PanelToggleButton : MonoBehaviour
{
    public GameObject panel;
    private Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(onClick);
    }

    public void onClick(bool value)
    {
        panel.SetActive(value);
    }
}

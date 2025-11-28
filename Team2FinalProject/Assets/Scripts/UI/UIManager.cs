using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public List<GameObject> setUpPanels = new List<GameObject>();
    public List<GameObject> playPanels = new List<GameObject>();
    public List<PanelScript> panels = new List<PanelScript>();
    public PanelScript rotPanel;
    public TMP_Text rotText;
    public TMP_Dropdown rotSceneNodeDropDown;


    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.gameModeChanged.AddListener(UpdateUILayout);
        AnimationManager.doneAnimation.AddListener(showRotPanel);
        UpdateUILayout();
    }

    public static void UpdateUILayout()
    {
        EGameMode gameMode = GameManager.GetGameMode();
        Debug.Log(gameMode);
        instance.rotText.gameObject.SetActive(false);
        instance.rotSceneNodeDropDown.gameObject.SetActive(true);
        for (int i = 0; i < instance.panels.Count; i++)
        {
            instance.panels[i].SetVisibility();
        }
        for (int i = 0; i < instance.setUpPanels.Count; i++)
        {
            instance.setUpPanels[i].SetActive(gameMode == EGameMode.SETUP);
        }

        for (int i = 0; i < instance.playPanels.Count; i++)
        {
            instance.playPanels[i].SetActive(gameMode == EGameMode.PLAY);
        }
    }

    public static void showRotPanel() 
    { 
        instance.rotPanel.SetVisibility(true);
        instance.rotText.gameObject.SetActive(true);
        instance.rotSceneNodeDropDown.gameObject.SetActive(false);
    }
}

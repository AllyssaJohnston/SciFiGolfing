using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public List<GameObject> setUpPanels = new List<GameObject>();
    public List<GameObject> playPanels = new List<GameObject>();
    public GameObject rotPanel;
    public GameObject rotPanelToggle;


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
        for (int i = 0; i < instance.setUpPanels.Count; i++)
        {
            instance.setUpPanels[i].SetActive(gameMode == EGameMode.SETUP);
            PanelScript panel = instance.setUpPanels[i].GetComponent<PanelScript>();
            if (panel != null) { panel.SetVisibility(gameMode == EGameMode.SETUP); }
        }

        for (int i = 0; i < instance.playPanels.Count; i++)
        {
            instance.playPanels[i].SetActive(gameMode == EGameMode.PLAY);
            PanelScript panel = instance.playPanels[i].GetComponent<PanelScript>();
            if (panel != null) { panel.SetVisibility(gameMode == EGameMode.PLAY); }
        }
    }

    public static void showRotPanel()
    {
        instance.rotPanel.SetActive(true);
        instance.rotPanelToggle.SetActive(true);
        instance.rotPanel.GetComponent<PanelScript>().SetVisibility(true);

    }
}

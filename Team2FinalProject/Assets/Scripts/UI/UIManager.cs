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
    private Vector2 setUpRotPos;
    public Vector2 playRotPos;
    private Vector3 toggleOffset;


    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        setUpRotPos = rotPanel.transform.position;
        toggleOffset = new Vector2(rotPanelToggle.transform.position.x, rotPanelToggle.transform.position.y) - setUpRotPos;
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
            Toggle toggle = instance.setUpPanels[i].GetComponent<Toggle>();
            if (toggle != null) { toggle.isOn = gameMode == EGameMode.SETUP; }
        }
        
        for (int i = 0; i < instance.playPanels.Count; i++)
        {
            instance.playPanels[i].SetActive(gameMode == EGameMode.PLAY);
            Toggle toggle = instance.playPanels[i].GetComponent<Toggle>();
            if (toggle != null) { toggle.isOn = gameMode == EGameMode.PLAY; }
        }
        instance.rotPanel.transform.position = (gameMode == EGameMode.SETUP) ? instance.setUpRotPos : instance.playRotPos;
        instance.rotPanelToggle.transform.position = instance.rotPanel.transform.position + instance.toggleOffset;
    }

    public static void showRotPanel()
    {
        instance.rotPanel.SetActive(true);
        instance.rotPanelToggle.SetActive(true);
        instance.rotPanelToggle.GetComponent<Toggle>().isOn = true;
    }
}

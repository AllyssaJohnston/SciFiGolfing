using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public List<GameObject> setUpPanels = new List<GameObject>();
    public List<GameObject> playPanels = new List<GameObject>();


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
        UpdateUILayout();
    }

    public static void UpdateUILayout()
    {
        EGameMode gameMode = GameManager.GetGameMode();
        Debug.Log(gameMode);
        for (int i = 0; i < instance.setUpPanels.Count; i++)
        {
            instance.setUpPanels[i].SetActive(gameMode == EGameMode.SETUP);
        }
        for (int i = 0; i < instance.playPanels.Count; i++)
        {
            instance.playPanels[i].SetActive(gameMode == EGameMode.PLAY);
        }
    }
}

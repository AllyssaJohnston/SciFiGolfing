using UnityEngine;
using UnityEngine.Events;

public enum EGameMode
{
    SETUP = 0,
    PLAY = 1,
}

public enum ELightMode
{
    DIFFUSE = 0,
    POINT = 1,
    DIFFUSE_AND_POINT = 2
}
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static EGameMode gameMode = EGameMode.SETUP;
    private static ELightMode lightMode = ELightMode.POINT;
    public static UnityEvent gameModeChanged;
    public static UnityEvent lightModeChanged;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        gameModeChanged = new UnityEvent();
        lightModeChanged = new UnityEvent();
    }



    public static EGameMode GetGameMode() { return gameMode; }

    public static ELightMode GetLightMode() { return lightMode; }


    public static void SetGameMode(EGameMode givenGameMode) 
    {
        Debug.Log("switching to " + givenGameMode);
        gameMode = givenGameMode;
        gameModeChanged.Invoke();
    }  
}

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
    DIFFUSE_AND_POINT = 2,
    NONE = 3
}
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static EGameMode lastGameMode = EGameMode.SETUP;
    private static EGameMode gameMode = EGameMode.SETUP;
    private static ELightMode lightMode = ELightMode.DIFFUSE_AND_POINT;
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
        Debug.Log(gameMode);
        Debug.Log(lightMode);
    }


    public static EGameMode GetLastGameMode() { return lastGameMode; }

    public static EGameMode GetGameMode() { return gameMode; }

    public static ELightMode GetLightMode() { return lightMode; }

    public static void SetGameMode(EGameMode givenGameMode) 
    {
        Debug.Log("switching to " + givenGameMode);
        lastGameMode = gameMode;
        gameMode = givenGameMode;
        gameModeChanged.Invoke();
    }

    public static void SetDiffuse(bool useDiffuse)
    { 
        switch(useDiffuse)
        {
        case true:
            switch(lightMode)
            {
            case ELightMode.NONE:
                SetLightMode(ELightMode.DIFFUSE);
                break;
            case ELightMode.POINT:
                SetLightMode(ELightMode.DIFFUSE_AND_POINT);
                break;
            default:
                break;
            }
            break;
        case false:
            switch (lightMode)
            {
            case ELightMode.DIFFUSE:
                SetLightMode(ELightMode.NONE);
                break;
            case ELightMode.DIFFUSE_AND_POINT:
                SetLightMode(ELightMode.POINT);
                break;
            default:
                break;
            }
            break;
        }
    }

    public static void SetLightMode(ELightMode givenLightMode)
    {
        Debug.Log("switching to " + givenLightMode);
        lightMode = givenLightMode;
        lightModeChanged.Invoke();
    }
}

using UnityEngine;
using UnityEngine.Events;

public enum EGameMode
{
    SETUP = 0,
    PLAY
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static EGameMode gameMode = EGameMode.SETUP;
    public static UnityEvent gameModeChanged;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        gameModeChanged = new UnityEvent();
    }



    public static EGameMode GetGameMode() { return gameMode; }


    public static void SetGameMode(EGameMode givenGameMode) 
    {
        Debug.Log("switching to " + givenGameMode);
        gameMode = givenGameMode;
        gameModeChanged.Invoke();
    }  
}

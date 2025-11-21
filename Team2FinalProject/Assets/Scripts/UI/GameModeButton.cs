using UnityEngine;

public class GameModeButton : MonoBehaviour
{
    public EGameMode gameMode;

    public void OnClick() { GameManager.SetGameMode(gameMode); }
}

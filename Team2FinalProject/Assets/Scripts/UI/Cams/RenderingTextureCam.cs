using UnityEngine;

public class RenderingTextureCam : MonoBehaviour
{
    public Camera cam;
    public RenderTexture setUp;
    public RenderTexture play;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.gameModeChanged.AddListener(SwitchTexture);
    }

    public void SwitchTexture()
    {
        EGameMode gameMode = GameManager.GetGameMode();
        switch (gameMode)
        {
            case EGameMode.SETUP:
                cam.targetTexture = setUp;
                break;
            case EGameMode.PLAY:
                cam.targetTexture = play;
                break;
            default:
                break;
        }
    }
}

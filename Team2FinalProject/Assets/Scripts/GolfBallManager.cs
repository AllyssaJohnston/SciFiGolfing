using System.Collections.Generic;
using UnityEngine;

public class GolfBallManager : MonoBehaviour
{
    private static GolfBallManager instance;
    private static List<GameObject> golfBalls = new List<GameObject>();
    public GameObject ballTemplate;
    private static int numCreated = 0;
    private static float duplicationRotation = 15f;

    public GameObject BallPrefab;
    public SceneNode SpawnPos;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void Start()
    {
        GameManager.gameModeChanged.AddListener(AddBall);
        ObjectManager.resetWorld.AddListener(Reset);
    }


    public static void AddBall()
    {
        Vector3 spawnPosForward = new Vector3(instance.SpawnPos.getXForm()[0, 2], instance.SpawnPos.getXForm()[1, 2], instance.SpawnPos.getXForm()[2, 2]).normalized * -1;
        Vector3 spawnPosRight = new Vector3(instance.SpawnPos.getXForm()[0, 0], instance.SpawnPos.getXForm()[1, 0], instance.SpawnPos.getXForm()[2, 0]).normalized * -1.25f;
        Vector3 spawnPosition = instance.SpawnPos.getXForm().GetPosition() + spawnPosForward + spawnPosRight;
        GameObject ball = Instantiate(instance.BallPrefab, spawnPosition, instance.SpawnPos.transform.rotation);
        numCreated++;
        ball.name = "Ball " + numCreated;
        golfBalls.Add(ball);
        ScoreTracker.increaseBalls();
    }

    public static void Duplicate()
    {
        
    }

    public static void Reset()
    {
        for (int i = golfBalls.Count - 1; i > -1; i--)
        {
            if (golfBalls[i] == null) { continue; }
            golfBalls[i].SetActive(false);
            Destroy(golfBalls[i]);
        }
        golfBalls.Clear();
    }

    public static float getDuplicationRotation() { return duplicationRotation; }

    public static void setDuplicationRotation(float rot) { duplicationRotation = rot; }
}

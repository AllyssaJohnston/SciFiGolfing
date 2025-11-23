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
    public LayerMask ballLayer;

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
        Vector3 rayStartForward = new Vector3(instance.SpawnPos.getXForm()[0, 2], instance.SpawnPos.getXForm()[1, 2], instance.SpawnPos.getXForm()[2, 2]).normalized * -1;
        Vector3 rayStartRight = new Vector3(instance.SpawnPos.getXForm()[0, 0], instance.SpawnPos.getXForm()[1, 0], instance.SpawnPos.getXForm()[2, 0]).normalized * -1.25f;
        Vector3 rayStartUp = new Vector3(instance.SpawnPos.getXForm()[0, 1], instance.SpawnPos.getXForm()[1, 1], instance.SpawnPos.getXForm()[2, 1]).normalized * -4f;
        Vector3 rayStartPos = instance.SpawnPos.getXForm().GetPosition() + rayStartForward + rayStartRight + rayStartUp;
        RaycastHit hit;
        bool hitBall = Physics.Raycast(rayStartPos, rayStartForward, out hit, 200, instance.ballLayer);
        if (hitBall){
            Debug.DrawRay(rayStartPos, rayStartForward * 200, Color.yellow);
            Vector3 ballOriginalPos = hit.transform.position;
            GameObject ball1 = Instantiate(instance.BallPrefab, ballOriginalPos, hit.transform.rotation);
            GameObject ball2 = Instantiate(instance.BallPrefab, ballOriginalPos, hit.transform.rotation);
            ScoreTracker.increaseBalls();
            ScoreTracker.increaseBalls();

            Rigidbody rb1 = ball1.GetComponent<Rigidbody>();
            Vector3 dir1 =  Quaternion.AngleAxis(duplicationRotation, rayStartUp) * rayStartForward;
            rb1.AddForce(dir1 * 7, ForceMode.Impulse);
            Rigidbody rb2 = ball2.GetComponent<Rigidbody>();
            Vector3 dir2 =  Quaternion.AngleAxis(-duplicationRotation, rayStartUp) *rayStartForward;
            rb2.AddForce(dir2 * 7, ForceMode.Impulse);
            hit.transform.gameObject.GetComponent<GolfBall>().Reset();
        }
        else{
            Debug.DrawRay(rayStartPos, rayStartForward * 200, Color.red);
        }

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

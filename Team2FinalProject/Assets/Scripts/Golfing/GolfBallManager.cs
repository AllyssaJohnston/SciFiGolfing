using JetBrains.Annotations;
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
    public GameObject BallParentObj;
    public SceneNode SpawnPos;
    public LayerMask ballLayer;

    private static int rayDist = 200;
    private static float rayTimer = 0f;
    private static float rayTimerLength = 2f;
    public GameObject rayCylinder;

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
        rayCylinder.SetActive(false);
        instance.rayCylinder.transform.localScale = new Vector3(.5f, rayDist / 2, .5f);
        GameManager.gameModeChanged.AddListener(AddBall);
        ObjectManager.resetWorld.AddListener(Reset);
    }

    public void Update()
    {
        rayTimer -= Time.deltaTime;
        if (rayTimer <= 0)
        {
            rayCylinder.SetActive(false);
        }
    }


    private static void AddBall()
    {
        Vector3 spawnPosForward = new Vector3(instance.SpawnPos.getXForm()[0, 2], instance.SpawnPos.getXForm()[1, 2], instance.SpawnPos.getXForm()[2, 2]).normalized * -1;
        Vector3 spawnPosRight = new Vector3(instance.SpawnPos.getXForm()[0, 0], instance.SpawnPos.getXForm()[1, 0], instance.SpawnPos.getXForm()[2, 0]).normalized * -1.25f;
        Vector3 spawnPosition = instance.SpawnPos.getXForm().GetPosition() + spawnPosForward + spawnPosRight;
        GameObject ball = AddBall(spawnPosition, instance.SpawnPos.transform.rotation);
        LightManager.changeLighting();

    }

    private static GameObject AddBall(Vector3 spawnPos, Quaternion rot)
    {
        GameObject ball = Instantiate(instance.BallPrefab, spawnPos, rot, instance.BallParentObj.transform);
        ball.GetComponent<GolfBall>().SetUp();
        numCreated++;
        ball.name = "Ball " + numCreated;
        golfBalls.Add(ball);
        ScoreTracker.increaseBalls();
        return ball;
    }

    public static void Duplicate()
    {
        Vector3 rayStartForward = new Vector3(instance.SpawnPos.getXForm()[0, 2], instance.SpawnPos.getXForm()[1, 2], instance.SpawnPos.getXForm()[2, 2]).normalized * -1;
        Vector3 rayStartRight = new Vector3(instance.SpawnPos.getXForm()[0, 0], instance.SpawnPos.getXForm()[1, 0], instance.SpawnPos.getXForm()[2, 0]).normalized * -1.25f;
        Vector3 rayStartUp = new Vector3(instance.SpawnPos.getXForm()[0, 1], instance.SpawnPos.getXForm()[1, 1], instance.SpawnPos.getXForm()[2, 1]).normalized * -4f;
        Vector3 rayStartPos = instance.SpawnPos.getXForm().GetPosition() + rayStartForward + rayStartRight + rayStartUp;

        instance.rayCylinder.SetActive(true);
        instance.rayCylinder.transform.position = rayStartPos;
        instance.rayCylinder.transform.forward = rayStartForward;
        rayTimer = rayTimerLength;

        bool hitAny = false;
        for (int i = -1; i <= 1; i++)
        {
            RaycastHit[] hits = Physics.RaycastAll(rayStartPos + (Vector3.right * i * .2f), rayStartForward + (Vector3.right * i * .2f), rayDist, instance.ballLayer);
            hitAny = hitAny || hits.Length > 0;
            foreach(RaycastHit hit in hits)
            {
                if (!hit.collider.gameObject.GetComponent<GolfBall>().collisionActive)
                {
                    continue;
                }
                Debug.DrawRay(rayStartPos, rayStartForward * rayDist, Color.yellow);
                Vector3 ballOriginalPos = hit.transform.position;
                hit.collider.gameObject.SetActive(false);
                GameObject ball1 = AddBall(ballOriginalPos, hit.transform.rotation);
                GameObject ball2 = AddBall(ballOriginalPos, hit.transform.rotation);
                
                Rigidbody rb1 = ball1.GetComponent<Rigidbody>();
                Vector3 dir1 = Quaternion.AngleAxis(duplicationRotation, rayStartUp) * rayStartForward;
                rb1.AddForce(dir1 * 7, ForceMode.Impulse);
                Rigidbody rb2 = ball2.GetComponent<Rigidbody>();
                Vector3 dir2 = Quaternion.AngleAxis(-duplicationRotation, rayStartUp) * rayStartForward;
                rb2.AddForce(dir2 * 7, ForceMode.Impulse);
                hit.transform.gameObject.GetComponent<GolfBall>().Reset();
            }
        }
        if (hitAny)
        {
            LightManager.changeLighting();
        }
        else
        {
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

    public static void getGlowingGolfBallsPos(ref List<Vector4> pointLightTrans)
    {
        int light = 0;
        for (int i =0; i < golfBalls.Count; i++)
        {
            if (golfBalls[i] == null) { continue; }
            GolfBall g = golfBalls[i].GetComponent<GolfBall>();
            if (g.isActiveAndEnabled && g.glowing())
            {
                pointLightTrans.Add(golfBalls[i].transform.position + Vector3.up * 2);
                light++;
                if (light >= 30)
                {
                    return;
                }
            }
        }
    }
}

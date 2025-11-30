using System.Collections.Generic;
using UnityEngine;

public class GolfBallManager : MonoBehaviour
{
    private static GolfBallManager instance;
    private static List<GolfBall> golfBalls = new List<GolfBall>();
    private static List<GameObject> golfBallsObjs = new List<GameObject>();
    public GameObject ballTemplate;
    private static int numCreated = 0;
    private static float duplicationRotation = 15f;
    private static float duplicationForce = 7f;

    public GameObject BallPrefab;
    public GameObject BallParentObj;
    public SceneNode SpawnPos;
    public LayerMask ballLayer;

    private static int rayDist = 200;
    private static float rayTimer = 0f;
    private static float rayTimerLength = 2f;
    public GameObject rayCylinder;

    private static Matrix4x4 lastSpawnPos = Matrix4x4.zero;

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
        ObjectManager.backToSetUp.AddListener(Reset);
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
        if (GameManager.GetGameMode() != EGameMode.PLAY) { return; }
        Vector3 spawnPosForward = new Vector3(instance.SpawnPos.getXForm()[0, 2], instance.SpawnPos.getXForm()[1, 2], instance.SpawnPos.getXForm()[2, 2]).normalized * -1;
        Vector3 spawnPosRight = new Vector3(instance.SpawnPos.getXForm()[0, 0], instance.SpawnPos.getXForm()[1, 0], instance.SpawnPos.getXForm()[2, 0]).normalized * -1.25f;
        Vector3 spawnPosition = instance.SpawnPos.getXForm().GetPosition() + spawnPosForward + spawnPosRight;
        GameObject ball = AddBall(spawnPosition, instance.SpawnPos.transform.rotation);
        LightManager.changeLighting();

    }

    private static GameObject AddBall(Vector3 spawnPos, Quaternion rot)
    {
        GameObject obj = Instantiate(instance.BallPrefab, spawnPos, rot, instance.BallParentObj.transform);
        GolfBall ball = obj.GetComponent<GolfBall>();
        numCreated++;
        ball.SetUp();
        obj.name = "Ball " + numCreated;
        golfBallsObjs.Add(obj);
        golfBalls.Add(ball);
        ScoreTracker.increaseBalls();
        GlowingManager.setUpGlow(ball);
        return obj;
    }

    public static void Duplicate()
    {
        Vector3 rayStartForward = new Vector3(instance.SpawnPos.getXForm()[0, 2], instance.SpawnPos.getXForm()[1, 2], instance.SpawnPos.getXForm()[2, 2]).normalized * -1;
        Vector3 rayStartRight = new Vector3(instance.SpawnPos.getXForm()[0, 0], instance.SpawnPos.getXForm()[1, 0], instance.SpawnPos.getXForm()[2, 0]).normalized * -1.25f;
        Vector3 rayStartUp = new Vector3(instance.SpawnPos.getXForm()[0, 1], instance.SpawnPos.getXForm()[1, 1], instance.SpawnPos.getXForm()[2, 1]).normalized * -4f;
        Vector3 rayStartPos = instance.SpawnPos.getXForm().GetPosition() + rayStartForward + rayStartRight + rayStartUp;

        lastSpawnPos = instance.SpawnPos.getXForm();

        instance.rayCylinder.SetActive(true);
        instance.rayCylinder.transform.position = rayStartPos;
        instance.rayCylinder.transform.forward = rayStartForward;
        rayTimer = rayTimerLength;

        bool hitAny = false;
        for (int i = -1; i <= 1; i++)
        {
            RaycastHit[] hits = Physics.RaycastAll(rayStartPos + (Vector3.right * i * .2f), rayStartForward + (Vector3.right * i * .2f), rayDist, instance.ballLayer);
            hitAny = hitAny || hits.Length > 0;
            foreach (RaycastHit hit in hits)
            {
                if (!hit.collider.gameObject.GetComponent<GolfBall>().collisionActive) {  continue; }
                Vector3 ballOriginalPos = hit.transform.position;
                hit.collider.gameObject.SetActive(false);

                GameObject ball1 = AddBall(ballOriginalPos, hit.transform.rotation);
                GameObject ball2 = AddBall(ballOriginalPos, hit.transform.rotation);
                
                Rigidbody rb1 = ball1.GetComponent<Rigidbody>();
                Vector3 dir1 = Quaternion.AngleAxis(duplicationRotation, rayStartUp) * rayStartForward;
                rb1.AddForce(dir1 * duplicationForce, ForceMode.Impulse);
                Rigidbody rb2 = ball2.GetComponent<Rigidbody>();
                Vector3 dir2 = Quaternion.AngleAxis(-duplicationRotation, rayStartUp) * rayStartForward;
                rb2.AddForce(dir2 * duplicationForce, ForceMode.Impulse);

                hit.transform.gameObject.GetComponent<GolfBall>().Reset();
            }
        }
        if (hitAny)
        {
            LightManager.changeLighting();
        }
    }

    public static void Reset()
    {
        golfBalls.Clear();
        for (int i = golfBallsObjs.Count - 1; i > -1; i--)
        {
            if (golfBallsObjs[i] == null) { continue; }
            golfBallsObjs[i].SetActive(false);
            Destroy(golfBallsObjs[i]);
        }
        golfBallsObjs.Clear();
    }

    public static float getDuplicationRotation() { return duplicationRotation; }

    public static void setDuplicationRotation(float rot) { duplicationRotation = rot; }

    public static float getDuplicationForce() { return duplicationForce; }

    public static void setDuplicationForce(float f) { duplicationForce = f; }
    
    public static List<GolfBall> getGolfBalls() { return golfBalls; }

    public static List<GameObject> getGolfBallObjs() { return golfBallsObjs; }

    public static void getGlowingGolfBallsPos(ref List<Vector4> pointLightPos, ref List<Vector4> nearFar, ref List<Vector4> color)
    {
        Vector4 ballNearFar = LightManager.GetBallNearFar();
        Vector4 ballColor = LightManager.GetBallColor();
        int light = pointLightPos.Count;
        for (int i = 0; i < golfBalls.Count; i++)
        {
            if (golfBalls[i] == null) { continue; }
            GolfBall g = golfBalls[i];
            if (g.isActiveAndEnabled && g.glowing())
            {
                pointLightPos.Add(golfBalls[i].gameObject.transform.position + Vector3.up * 2);
                nearFar.Add(ballNearFar);
                color.Add(ballColor);
                light++;
                if (light >= 30)
                {
                    return;
                }
            }
        }
    }

    public static void getRayGolfBallsPos(ref List<Vector4> pointLightPos, ref List<Vector4> nearFar, ref List<Vector4> color)
    {
        if (rayTimer > 0)
        {
            Vector4 rayNearFar = LightManager.GetRayNearFar();
            Vector4 rayColor = LightManager.GetRayColor();
            int light = pointLightPos.Count;

            Vector3 rayForward = new Vector3(lastSpawnPos[0, 2], lastSpawnPos[1, 2], lastSpawnPos[2, 2]).normalized * -1;
            Vector3 rayStartRight = new Vector3(lastSpawnPos[0, 0], lastSpawnPos[1, 0], lastSpawnPos[2, 0]).normalized * -1.25f;
            Vector3 rayStartUp = new Vector3(lastSpawnPos[0, 1], lastSpawnPos[1, 1], lastSpawnPos[2, 1]).normalized * -4f;
            Vector3 rayStartPos = lastSpawnPos.GetPosition() + rayForward + rayStartRight + rayStartUp + Vector3.up * .5f;

            float length = instance.rayCylinder.transform.localScale.y * 2;
            int spacing = 3;
            for (int i = 0; i < Mathf.Ceil(length / spacing) - 1; i++)
            {
                pointLightPos.Add(rayStartPos + rayForward * i * spacing);
                nearFar.Add(rayNearFar);
                color.Add(rayColor);
                light++;
                if (light >= 30)
                {
                    return;
                }
            }
            pointLightPos.Add(rayStartPos + rayForward * length);
        }
    }

}

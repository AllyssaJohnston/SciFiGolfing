using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GolfBallManager : MonoBehaviour
{
    private static GolfBallManager instance;
    private static List<GameObject> golfBalls = new List<GameObject>();
    public GameObject ballTemplate;
    private static int numCreated = 0;
    private static float duplicationRotation = 15f;

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
        //GameObject g = Instantiate(instance.ballTemplate);
        //g.SetActive(true);
        //numCreated++;
        //g.name = "GolfBall" + numCreated;
        //g.transform.parent = instance.ballTemplate.transform.parent;
        //golfBalls.Add(g);
    }

    private static GameObject AddBall(GameObject org)
    {
        GameObject g = GameObject.Instantiate(org, instance.ballTemplate.transform.parent);
        numCreated++;
        g.name = "GolfBall" + numCreated;
        golfBalls.Add(g);
        return g;
    }

    public static void Duplicate()
    {
        //for (int i = golfBalls.Count - 1; i > -1; i--)
        //{
        //    if (golfBalls[i] == null) { continue; }
        //    Vector3 startingPos = golfBalls[i].transform.position;
        //    Vector3 dir = new Vector3(golfBalls[i].transform.forward.x, 0, golfBalls[i].transform.forward.z).normalized;
        //    Debug.Log(dir);
        //    Rigidbody orgRB = golfBalls[i].GetComponent<Rigidbody>();
        //    float v = orgRB.linearVelocity.magnitude;

        //    golfBalls[i].GetComponent<GolfBall>().Rotate(duplicationRotation, v, dir);

        //    GameObject twin = AddBall(golfBalls[i]);
        //    twin.transform.position = startingPos;
        //    twin.GetComponent<GolfBall>().SetUp();

        //    twin.GetComponent<GolfBall>().Rotate(-1 * duplicationRotation, v, dir);
        //}

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

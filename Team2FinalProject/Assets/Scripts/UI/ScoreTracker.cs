using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreTracker : MonoBehaviour
{

    int score = 0;
    int totalBalls = 1;

    public TMP_Text scoreTxt;
    public TMP_Text ballsTxt;

    public GameObject BallPrefab;
    public SceneNode SpawnPos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(){
        score = 0;
        totalBalls = 0;

        

        scoreTxt.text = "Score: " + score;
        ballsTxt.text = "Balls: " + totalBalls;
    }

    public void Play(){
        Vector3 spawnPosForward = new Vector3(SpawnPos.getXForm()[0,2],SpawnPos.getXForm()[1,2],SpawnPos.getXForm()[2,2]).normalized * -1;
        Vector3 spawnPosRight = new Vector3(SpawnPos.getXForm()[0,0],SpawnPos.getXForm()[1,0],SpawnPos.getXForm()[2,0]).normalized * -1.25f;
        Vector3 spawnPosition = SpawnPos.getXForm().GetPosition() + spawnPosForward + spawnPosRight;
        Instantiate(BallPrefab, spawnPosition, SpawnPos.transform.rotation);
    }

    public void IncreaseScore(){
        score += 1;
        scoreTxt.text = "Score: " + score;
    }

    public void decreaseBalls(){
        totalBalls -= 1;
        ballsTxt.text = "Balls: " + totalBalls;
    }

    public void increaseBalls(){
        totalBalls += 1;
        ballsTxt.text = "Balls: " + totalBalls;
    }
}

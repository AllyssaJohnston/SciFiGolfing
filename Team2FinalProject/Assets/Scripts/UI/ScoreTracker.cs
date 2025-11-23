using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
    private static ScoreTracker instance;
    static int score = 0;
    static int totalBalls = 1;

    public TMP_Text scoreTxt;
    public TMP_Text ballsTxt;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Reset();
    }

    public static void Reset(){
        score = 0;
        totalBalls = 0;

        instance.scoreTxt.text = "Score: " + score;
        instance.ballsTxt.text = "Balls: " + totalBalls;
    }

    public static void Play(){
        
    }

    public static void IncreaseScore(){
        score += 1;
        instance.scoreTxt.text = "Score: " + score;
    }

    public static void decreaseBalls(){
        totalBalls -= 1;
        instance.ballsTxt.text = "Balls: " + totalBalls;
    }

    public static void increaseBalls(){
        totalBalls += 1;
        instance.ballsTxt.text = "Balls: " + totalBalls;
    }
}

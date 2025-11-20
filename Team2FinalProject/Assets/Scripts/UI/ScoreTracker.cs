using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreTracker : MonoBehaviour
{

    int score = 0;
    int totalBalls = 1;

    public TMP_Text scoreTxt;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreTxt.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(){
        score = 0;
        totalBalls = 1;
    }

    public void IncreaseScore(){
        score += 1;
        scoreTxt.text = "Score: " + score;
    }
}

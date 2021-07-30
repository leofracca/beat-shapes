using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreWindow : MonoBehaviour
{
    private static ScoreWindow scoreWindow;
    private TextMeshProUGUI score;

    void Start()
    {
        scoreWindow = this;

        score = transform.Find("Score").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        int currentScore = Level.obstaclesPassed / 2; // We need to divide by 2 because it increment the score for 2 obstacle every time
        score.text = currentScore.ToString();
    }

    public static void HideScoreOverWindow()
    {
        scoreWindow.gameObject.SetActive(false);
    }
}

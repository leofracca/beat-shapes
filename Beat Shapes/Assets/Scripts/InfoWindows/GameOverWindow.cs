using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverWindow : MonoBehaviour
{
    private static GameOverWindow gameOverWindow;

    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI highScore;

    void Start()
    {
        gameOverWindow = this;

        HideGameOverWindow();
    }

    void HideGameOverWindow()
    {
        gameObject.SetActive(false);
    }

    // Show the game over window and calculate the score and the highscore
    public static void ShowGameOverWindow()
    {
        gameOverWindow.finalScore.text = gameOverWindow.GetScore().ToString();

        gameOverWindow.SetHighScore();
        gameOverWindow.highScore.text = PlayerPrefs.GetInt("highscore", 0).ToString();

        gameOverWindow.gameObject.SetActive(true);
    }

    private int GetScore()
    {
        int currentScore = Level.obstaclesPassed / 2; // We need to divide by 2 because it increment the score for 2 obstacle every time

        return currentScore;
    }

    private void SetHighScore()
    {
        int score = GetScore();

        if (score > PlayerPrefs.GetInt("highscore", 0))
        {
            PlayerPrefs.SetInt("highscore", score);
        }
    }

    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LevelChanger.Instance.FadeToScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        LevelChanger.Instance.FadeToScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}

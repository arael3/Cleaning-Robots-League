using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMenus : MonoBehaviour
{
    [SerializeField] GameObject resumeButton;

    [SerializeField] TextMeshProUGUI GameScreenTitle;

    public static bool ifPause = false;

    public void Setup(bool ifPause)
    {
        if (ifPause)
        {
            GameScreenTitle.text = "PAUSE";
        }
        else
        {
            GameScreenTitle.text = "GAME OVER";
            resumeButton.SetActive(false);
        }

        gameObject.SetActive(true);
    }

    public void GamePause()
    {
        if (MatchTime.matchDuration > 0)
        {
            Setup(true);
            Time.timeScale = 0;
            ifPause = true;
        }
    }

    public void ResumeMatch()
    {
        if (MatchTime.matchDuration > 0)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            ifPause = false;
        }
    }
    public void StartMatch()
    {
        SceneManager.LoadScene("Game");
    }

    public void RestartMatch()
    {
        MatchTime.matchDuration = 300;
        Score.teamBlueScore = 0;
        Score.teamBlueScore = 0;
        SceneManager.LoadScene("Game");
        gameObject.SetActive(false);
        resumeButton.SetActive(false);
        Time.timeScale = 1;
        ifPause = false;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}



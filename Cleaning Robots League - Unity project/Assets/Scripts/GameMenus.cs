using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMenus : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject matchStartsIn;
    

    [SerializeField] GameObject timeBeforeMatchStarts;
    TimeBeforeMatchStarts timeBeforeMatchStartsScript;

    [SerializeField] GameObject countingTimeAfterGoal;
    CountingTimeAfterGoal countingTimeAfterGoalScript;

    [SerializeField] GameObject score;
    [SerializeField] GameObject restart;
    [SerializeField] GameObject exit;

    [SerializeField] TextMeshProUGUI GameScreenTitle;

    [HideInInspector]
    public bool ifStart = false;

    public static bool ifPause = false;
    

    public string test = "GameMenus";


    private void Start()
    {
        if (timeBeforeMatchStarts)
        {
            timeBeforeMatchStartsScript = timeBeforeMatchStarts.GetComponent<TimeBeforeMatchStarts>();
        }
    }

    public void IfPause(bool ifPause)
    {
        if (ifPause)
        {
            GameScreenTitle.text = "PAUSE";
        }
        else
        {
            GameScreenTitle.text = "GAME OVER";
            resumeButton.SetActive(false);
            countingTimeAfterGoal.SetActive(false);
        }

        gameObject.SetActive(true);
    }

    public void GamePause()
    {
        if (MatchTime.matchDuration > 0)
        {
            IfPause(true);
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
    
    public void StartCounting()
    {
        ifStart = true;
        matchStartsIn.SetActive(true);
        timeBeforeMatchStarts.SetActive(true);

        if (bomb)
        {
            bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        

        if (resumeButton)
        {
            resumeButton.SetActive(false);
        }
        if (score)
        {
            score.SetActive(false);
        }
        if (restart)
        {
            restart.SetActive(false);
        }
        
        exit.SetActive(false);

        Time.timeScale = 1;
    }

    public void HideStartButton()
    {
        startButton.SetActive(false);
    }

    //public void RestartMatch()
    //{
    //    MatchTime.matchDuration = 300;
    //    Bomb.teamBlueScore = 0;
    //    Bomb.teamRedScore = 0;
    //    SceneManager.LoadScene("Game");
    //    gameObject.SetActive(false);
    //    Time.timeScale = 1;
    //    ifPause = false;
    //}

    float time = 3.99f;
    public void CountingDownAndStartMatch()
    {
        if (time > 1.1f)
        {
            MatchTime.matchDuration = 5;
            time -= Time.deltaTime;
            timeBeforeMatchStartsScript.Time = (int)time;
            //Debug.Log("time = " + time);
        }
        else
        {
            MatchTime.matchDuration = 5;
            Bomb.teamBlueScore = 0;
            Bomb.teamRedScore = 0;

            ifPause = false;
            
            ifStart = false;
            GameController.ifGameOver = false;
            time = 3.99f;

            SceneManager.LoadScene("Game");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}



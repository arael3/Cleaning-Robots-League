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
    //TimeBeforeMatchStarts timeBeforeMatchStartsScript;

    [SerializeField] GameObject countingTimeAfterGoal;
    //CountingTimeAfterGoal countingTimeAfterGoalScript;

    [SerializeField] GameObject score;
    [SerializeField] GameObject restart;
    [SerializeField] GameObject exit;

    [SerializeField] TextMeshProUGUI GameScreenTitle;

    [HideInInspector]
    public static bool ifStart = false;

    public static bool ifPause = false;
    

    public string test = "GameMenus";


    private void Start()
    {
        //if (timeBeforeMatchStarts)
        //{
            //timeBeforeMatchStartsScript = timeBeforeMatchStarts.GetComponent<TimeBeforeMatchStarts>();
        //}
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
        }

        gameObject.SetActive(true);
    }

    public void GamePause()
    {
        if (MatchTime.matchDuration > 0)
        {
            countingTimeAfterGoal.SetActive(false);
            IfPause(true);
            Time.timeScale = 0;
            ifPause = true;
        }
    }

    public void ResumeMatch()
    {
        if (MatchTime.matchDuration > 0)
        {
            Time.timeScale = 1;
            if (CountingTimeAfterGoal.ifPauseAfterGoal)
            {
                countingTimeAfterGoal.SetActive(true);
            }
            ifPause = false;
            gameObject.SetActive(false);
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

    static float time = 3.99f;
    public static void CountingDownAndStartMatch()
    {
        if (time > 1.1f)
        {
            MatchTime.matchDuration = MatchTime.matchDurationRestart;
            time -= Time.deltaTime;
            TimeBeforeMatchStarts.time = (int)time;
        }
        else
        {
            MatchTime.matchDuration = MatchTime.matchDurationRestart;
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



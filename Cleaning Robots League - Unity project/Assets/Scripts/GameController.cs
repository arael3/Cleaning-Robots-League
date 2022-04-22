using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameMenus pauseMenu;
    [SerializeField] GameObject countingTimeAfterGoal;
    //GameMenus gameMenusScript;

    public static bool ifGameOver = false;

    void Start()
    {
        //gameMenusScript = pauseMenu.GetComponent<GameMenus>();
    }

    void Update()
    {
        if (MatchTime.matchDuration <= 0)
        {
            if (!ifGameOver)
            {
                countingTimeAfterGoal.SetActive(false);
                Time.timeScale = 0;
                pauseMenu.IfPause(false);
                ifGameOver = true;
                CountingTimeAfterGoal.ifPauseAfterGoal = false;
                Bomb.afterGoal = false;
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (GameMenus.ifPause)
            {
                pauseMenu.ResumeMatch();
            }
            else
            {
                pauseMenu.GamePause();
            }
        }

        if (GameMenus.ifStart)
        {
            GameMenus.CountingDownAndStartMatch();
        }
    }
}

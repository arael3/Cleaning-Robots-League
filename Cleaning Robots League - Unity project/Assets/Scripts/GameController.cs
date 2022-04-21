using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameMenus pauseMenu;
    GameMenus gameMenusScript;

    public static bool ifGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0;
        //SceneManager.LoadScene("Menu");

        gameMenusScript = pauseMenu.GetComponent<GameMenus>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Time.timeScale " + Time.timeScale);
        if (MatchTime.matchDuration <= 0)
        {
            if (!ifGameOver)
            {
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

        if (gameMenusScript.ifStart)
        {
            gameMenusScript.CountingDownAndStartMatch();
        }
    }
}

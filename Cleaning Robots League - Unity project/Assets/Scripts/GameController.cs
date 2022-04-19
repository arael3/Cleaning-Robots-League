using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameMenus pauseMenu;
    GameMenus gameMenusScript;

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
        if (MatchTime.matchDuration <= 0)
        {
            Time.timeScale = 0;
            pauseMenu.Setup(false);
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

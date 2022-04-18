using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameMenus PauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0;
        //SceneManager.LoadScene("Menu");
    }

    // Update is called once per frame
    void Update()
    {
        if (MatchTime.matchDuration <= 0)
        {
            Time.timeScale = 0;
            PauseMenu.Setup(false);
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (GameMenus.ifPause)
            {
                PauseMenu.ResumeMatch();
            }
            else
            {
                PauseMenu.GamePause();
            }
        }
    }
}

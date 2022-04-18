using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameMenus PauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        
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
                PauseMenu.Resume();
            }
            else
            {
                PauseMenu.GamePause();
            }
        }
    }
}

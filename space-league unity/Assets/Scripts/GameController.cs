using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameScreen GameScreen;
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
            GameScreen.Setup(false);
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (GameScreen.ifPause)
            {
                GameScreen.ResumeButton();
            }
            else
            {
                GameScreen.GamePause();
            }
        }
    }
}

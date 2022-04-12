using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldBombTimerBar : MonoBehaviour
{
    [SerializeField] GameObject teamBluePlayer;

    [SerializeField] GameObject teamRedPlayer;

    Slider holdBombTimerBar;

    float hBtimer;

    void Start()
    {
        holdBombTimerBar = GetComponent<Slider>();
    }

    void Update()
    {
       

        if (gameObject.name == "TeamBluePlayerHoldBombSlider")
        {
            hBtimer = Mathf.Round(teamBluePlayer.GetComponent<PlayerController>().HoldBombTimer * 100f) / 100f;
        }
        
        if (gameObject.name == "TeamRedPlayerHoldBombSlider")
        {
            hBtimer = Mathf.Round(teamRedPlayer.GetComponent<PlayerController>().HoldBombTimer * 100f) / 100f;
        }

        holdBombTimerBar.value = hBtimer / PlayerController.restartholdBombTimer;
    }
}

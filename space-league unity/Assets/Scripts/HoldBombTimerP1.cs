using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldBombTimerP1 : MonoBehaviour
{
    [SerializeField] GameObject teamBluePlayer;
    [SerializeField] GameObject teamRedPlayer;

    Text holdBombTimer;

    float hBtimer;
    // Start is called before the first frame update
    void Start()
    {
        holdBombTimer = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //hBtimer = Mathf.Round(PlayerController.holdBombTimer * 100f) / 100f;

        if (gameObject.name == "TeamBlueHoldBombTimerPlayer")
        {
            hBtimer = Mathf.Round(teamBluePlayer.GetComponent<PlayerController>().HoldBombTimer * 100f) / 100f;
        }

        if (gameObject.name == "TeamRedHoldBombTimerPlayer")
        {
            hBtimer = Mathf.Round(teamRedPlayer.GetComponent<PlayerController>().HoldBombTimer * 100f) / 100f;
        }

        holdBombTimer.text = hBtimer.ToString();
    }
}

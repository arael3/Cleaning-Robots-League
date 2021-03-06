using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldBombTimerCanvas : MonoBehaviour
{
    [SerializeField] GameObject teamBlueNPCTop;
    [SerializeField] GameObject teamBlueNPCMiddle;
    [SerializeField] GameObject teamBluePlayer;
    [SerializeField] GameObject teamBlueNPCBottom;
   
    [SerializeField] GameObject teamRedNPCTop;
    [SerializeField] GameObject teamRedNPCMiddle;
    [SerializeField] GameObject teamRedPlayer;
    [SerializeField] GameObject teamRedNPCBottom;

    Text holdBombTimer;

    Slider holdBombTimerBar;

    float hBtimer;
    // Start is called before the first frame update
    void Start()
    {
        holdBombTimer = GetComponent<Text>();
        //holdBombTimerBar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //hBtimer = Mathf.Round(NPCController.holdBombTimer * 100f) / 100f;

        if (gameObject.name == "TeamBlueHoldBombTimerNPCTop")
        {
            hBtimer = Mathf.Round(teamBlueNPCTop.GetComponent<NPCController>().HoldBombTimer * 100f) / 100f;
        }

        if (gameObject.name == "TeamBlueHoldBombTimerNPCMiddle")
        {
            hBtimer = Mathf.Round(teamBlueNPCMiddle.GetComponent<NPCController>().HoldBombTimer * 100f) / 100f;
        }

        if (gameObject.name == "TeamBlueHoldBombTimerPlayer")
        {
            hBtimer = Mathf.Round(teamBluePlayer.GetComponent<PlayerController>().HoldBombTimer * 100f) / 100f;
        }

        if (gameObject.name == "TeamBlueHoldBombTimerNPCBottom")
        {
            hBtimer = Mathf.Round(teamBlueNPCBottom.GetComponent<NPCController>().HoldBombTimer * 100f) / 100f;
        }

        if (gameObject.name == "TeamRedHoldBombTimerNPCTop")
        {
            hBtimer = Mathf.Round(teamRedNPCTop.GetComponent<NPCController>().HoldBombTimer * 100f) / 100f;
        }

        if (gameObject.name == "TeamRedHoldBombTimerNPCMiddle")
        {
            hBtimer = Mathf.Round(teamRedNPCMiddle.GetComponent<NPCController>().HoldBombTimer * 100f) / 100f;
        }

        if (gameObject.name == "TeamRedHoldBombTimerPlayer")
        {
            hBtimer = Mathf.Round(teamRedPlayer.GetComponent<PlayerController>().HoldBombTimer * 100f) / 100f;
        }

        if (gameObject.name == "TeamRedHoldBombTimerNPCBottom")
        {
            hBtimer = Mathf.Round(teamRedNPCBottom.GetComponent<NPCController>().HoldBombTimer * 100f) / 100f;
        }

        holdBombTimer.text = hBtimer.ToString();
        
        //holdBombTimerBar.value = hBtimer / PlayerController.restartholdBombTimer;
    }
}

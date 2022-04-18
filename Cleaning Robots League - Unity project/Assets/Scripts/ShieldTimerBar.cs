using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldTimerBar : MonoBehaviour
{
    [SerializeField] GameObject teamBluePlayer;

    [SerializeField] GameObject teamRedPlayer;

    Slider shieldTimerBar;

    float shieldTimer;

    void Start()
    {
        shieldTimerBar = GetComponent<Slider>();
    }

    void Update()
    {


        if (gameObject.name == "TeamBluePlayerShieldSlider")
        {
            shieldTimer = Mathf.Round(teamBluePlayer.GetComponent<PlayerController>().waitForShield * 100f) / 100f;
        }

        if (gameObject.name == "TeamRedPlayerShieldSlider")
        {
            shieldTimer = Mathf.Round(teamRedPlayer.GetComponent<PlayerController>().waitForShield * 100f) / 100f;
        }

        shieldTimerBar.value = 1 - shieldTimer / PlayerController.waitForShieldRestart;
    }
}

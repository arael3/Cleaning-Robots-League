using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTimerBar : MonoBehaviour
{
    [SerializeField] GameObject teamBluePlayer;

    [SerializeField] GameObject teamRedPlayer;

    Slider attackTimerBar;

    float attackTimer;

    void Start()
    {
        attackTimerBar = GetComponent<Slider>();
    }

    void Update()
    {


        if (gameObject.name == "TeamBluePlayerAttackSlider")
        {
            attackTimer = Mathf.Round(teamBluePlayer.GetComponent<PlayerController>().waitForAttack * 100f) / 100f;
        }

        if (gameObject.name == "TeamRedPlayerAttackSlider")
        {
            attackTimer = Mathf.Round(teamRedPlayer.GetComponent<PlayerController>().waitForAttack * 100f) / 100f;
        }

        attackTimerBar.value = 1 - attackTimer / PlayerController.waitForAttackRestart;
    }
}


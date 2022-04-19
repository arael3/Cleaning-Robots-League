using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    Text score;
    
    void Start()
    {
        score = GetComponent<Text>();
    }

    void Update()
    {
        score.text = Bomb.teamBlueScore + " : " + Bomb.teamRedScore;
    }
}

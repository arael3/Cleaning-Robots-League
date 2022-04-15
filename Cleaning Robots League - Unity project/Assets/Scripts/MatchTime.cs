using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchTime : MonoBehaviour
{
    Text matchTime;

    public static int matchDuration = 300;

    float second;
    
    // Start is called before the first frame update
    void Start()
    {
        matchTime = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        second += Time.deltaTime;
        if (second >= 1)
        {
            matchDuration--;
            second = 0;
        }
        
        matchTime.text = matchDuration.ToString();
    }
}

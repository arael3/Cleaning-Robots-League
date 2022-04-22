using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountingTimeAfterGoal : MonoBehaviour
{
    float time = 3.99f;

    public static bool ifPauseAfterGoal = false;

    TextMeshProUGUI countingTimeAfterGoal;

    void Start()
    {
        countingTimeAfterGoal = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (ifPauseAfterGoal)
        {
            CountingAfterGoal();
        }
    }

    public void CountingAfterGoal()
    {
        if (time > 1.1f)
        {
            time -= Time.deltaTime;
            int timeInt = (int)time;
            countingTimeAfterGoal.text = timeInt.ToString();
        }
        else
        {
            ifPauseAfterGoal = false;
            time = 3.99f;
            gameObject.SetActive(false);
        }
    }
}

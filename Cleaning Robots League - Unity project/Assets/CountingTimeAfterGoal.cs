using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountingTimeAfterGoal : MonoBehaviour
{
    //[SerializeField] GameObject bomb;

    float time = 3.99f;

    public static bool ifPauseAfterGoal = false;

    TextMeshProUGUI countingTimeAfterGoal;
    //TextMeshPro countingTimeAfterGoal;

    public string test = "CountingTimeAfterGoal";

    // Start is called before the first frame update
    void Start()
    {
        countingTimeAfterGoal = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
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

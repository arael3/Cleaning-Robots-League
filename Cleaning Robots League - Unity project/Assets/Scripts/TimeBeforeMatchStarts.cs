using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeBeforeMatchStarts : MonoBehaviour
{
    public static int time;

    public int Time
    {
        get { return time; }
        set { time = value; }
    }

    TextMeshProUGUI timeBeforeMatchStarts;

    // Start is called before the first frame update
    void Start()
    {
        timeBeforeMatchStarts = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timeBeforeMatchStarts.text = time.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleTimer : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    public float StartTime { get; private set; }
    public float StopTime { get; private set; }

    private bool _stop = false;

    void Start()
    {
        StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (_stop)
            return;

        var accTime = Time.time - StartTime;
        int h = (int)accTime / 3600;
        int m = (int)accTime / 60 % 60;
        int s = (int)accTime % 60;

        _text.text = string.Format("Time : {0:D2} : {1:D2} : {2:D2}", h, m, s);
    }

    void Stop()
    {
        _stop = true;
        StopTime = Time.time;
    }
}

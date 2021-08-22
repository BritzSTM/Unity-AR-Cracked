using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningAlarm : MonoBehaviour
{
    public bool ActiveAlarm = false;
    [SerializeField] private Image _alarmBG;
    public float BlinkTime = 1.0f;
    private float _accDeltaTime = 0.0f;

    void Update()
    {
        if(!ActiveAlarm)
        {
            _alarmBG.color = Color.green;
            return;
        }

        Color color = new Color();
        color.r = 1.0f;
        color.a = Mathf.Lerp(0.0f, 1.0f, Mathf.Sin((_accDeltaTime / BlinkTime) * Mathf.PI));

        _alarmBG.color = color;
        _accDeltaTime += Time.deltaTime;
        if (_accDeltaTime > BlinkTime)
            _accDeltaTime = 0.0f;
    }
}

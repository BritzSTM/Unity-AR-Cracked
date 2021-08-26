using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WarningAlarm))]
public class CountWarningAgent : MonoBehaviour
{
    public float warningRate = 0.7f;

    [SerializeField] private EventTypeGameManager _onUpdateDimCountSO;
    private WarningAlarm _alarm;

    private void Awake()
    {
        _alarm = GetComponent<WarningAlarm>();

        Debug.Assert(_alarm != null);
    }

    private void OnEnable()
    {
        _onUpdateDimCountSO.OnEvent += OnUpdate;
    }

    private void OnDisable()
    {
        _onUpdateDimCountSO.OnEvent -= OnUpdate;
    }

    private void OnUpdate(GameManager manager)
    {
        var rate = manager.CurrentDimCount / (float)manager.LimitDimCount;

        if (rate >= warningRate)
            _alarm.ActiveAlarm = true;
        else
            _alarm.ActiveAlarm = false;
    }
}

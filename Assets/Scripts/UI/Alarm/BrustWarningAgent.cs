using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WarningAlarm))]
public class BrustWarningAgent : MonoBehaviour
{
    [SerializeField] private EventTypeCrack _onBrustEventSO;
    [SerializeField] private EventTypeCrack _onUnbrustEventSO;

    private WarningAlarm _alarm;
    private List<Crack> _brustList = new List<Crack>();

    private void Awake()
    {
        _alarm = GetComponent<WarningAlarm>();
    }

    private void OnEnable()
    {
        _onBrustEventSO.OnEvent += OnBrustEvent;
        _onUnbrustEventSO.OnEvent += OnUnbrustEvent;
    }

    private void OnDisable()
    {
        _onBrustEventSO.OnEvent -= OnBrustEvent;
        _onUnbrustEventSO.OnEvent -= OnUnbrustEvent;
    }

    private void OnBrustEvent(Crack crack)
    {
        _brustList.Add(crack);
        _alarm.ActiveAlarm = true;
    }

    private void OnUnbrustEvent(Crack crack)
    {
        _brustList.Remove(crack);

        if (_brustList.Count == 0)
            _alarm.ActiveAlarm = false;
    }
}

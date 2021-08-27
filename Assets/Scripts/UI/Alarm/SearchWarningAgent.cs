using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WarningAlarm))]
public class SearchWarningAgent : MonoBehaviour
{
    [SerializeField] private EventTypeCrackManager _onCrackDeployModeSO;
    [SerializeField] private EventTypeCrackManager _onCrackUndeployModeSO;

    private WarningAlarm _alarm;
    private List<CrackManager> _trackingManagers = new List<CrackManager>();

    private void Awake()
    {
        _alarm = GetComponent<WarningAlarm>();

        Debug.Assert(_alarm != null);
    }

    private void OnEnable()
    {
        _onCrackDeployModeSO.OnEvent += OnDeployMode;
        _onCrackUndeployModeSO.OnEvent += OnUndeployMode;
    }

    private void OnDisable()
    {
        _onCrackDeployModeSO.OnEvent -= OnDeployMode;
        _onCrackUndeployModeSO.OnEvent -= OnUndeployMode;
    }

    private void OnDeployMode(CrackManager manager)
    {
        _trackingManagers.Add(manager);
        _alarm.ActiveAlarm = true;
    }

    private void OnUndeployMode(CrackManager manager)
    {
        _trackingManagers.Remove(manager);
        if (_trackingManagers.Count == 0)
            _alarm.ActiveAlarm = false;
    }
}

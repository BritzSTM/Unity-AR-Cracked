using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWhenSearchMode : MonoBehaviour
{
    [SerializeField] private EventTypeVoid _OnNormalModeEventSO;
    [SerializeField] private EventTypeVoid _OnSearchModeEventSO;
    [SerializeField] private GameObject[] _targets;

    private void OnEnable()
    {
        _OnNormalModeEventSO.OnEvent += OnNormalMode;
        _OnSearchModeEventSO.OnEvent += OnSearchMode;
    }

    private void OnDisable()
    {
        _OnNormalModeEventSO.OnEvent -= OnNormalMode;
        _OnSearchModeEventSO.OnEvent -= OnSearchMode;
    }

    private void SetTargetActives(bool b)
    {
        for (int i = 0; i < _targets.Length; ++i)
            _targets[i].SetActive(b);
    }

    private void OnNormalMode() => SetTargetActives(true);

    private void OnSearchMode() => SetTargetActives(false);
}

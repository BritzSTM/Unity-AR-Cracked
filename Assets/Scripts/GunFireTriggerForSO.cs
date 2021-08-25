using UnityEngine;

public class GunFireTriggerForSO : MonoBehaviour
{
    [Header("ReciveEvents")]
    [SerializeField] private EventTypeVoid _normalModeEventSO;
    [SerializeField] private EventTypeVoid _searchModeEventSO;

    [Header("RaiseEvents")]
    [SerializeField] private EventTypeVoid _leftGunFireSO;
    [SerializeField] private EventTypeVoid _RightGunFireSO;

    [Space()]
    [SerializeField] private bool _forbiddenAttackBySearchMode = true;

    private bool _isSearchMode;

    private void Awake()
    {
        Debug.Assert(_normalModeEventSO != null && _searchModeEventSO != null);
        Debug.Assert(_leftGunFireSO != null && _RightGunFireSO != null);
    }

    private void OnEnable()
    {
        _normalModeEventSO.OnEvent += OnNormalMode;
        _searchModeEventSO.OnEvent += OnSearchMode;
    }

    private void OnDisable()
    {
        _normalModeEventSO.OnEvent -= OnNormalMode;
        _searchModeEventSO.OnEvent -= OnSearchMode;
    }

    public void FireLeftGun()
    {
        if (_forbiddenAttackBySearchMode && _isSearchMode)
            return;

        _leftGunFireSO?.RaiseEvent();
    }

    public void FireRightGun()
    {
        if (_forbiddenAttackBySearchMode && _isSearchMode)
            return;

        _RightGunFireSO?.RaiseEvent();
    }

    private void OnNormalMode() => _isSearchMode = false;
    private void OnSearchMode() => _isSearchMode = true;
}

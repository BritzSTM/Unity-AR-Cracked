using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunFireManager : MonoBehaviour
{
    [Header("ReciveEvents")]
    [SerializeField] private EventTypeVoid _normalModeEventSO;
    [SerializeField] private EventTypeVoid _searchModeEventSO;
    [SerializeField] private EventTypeDim _onSpawnDimEventSO;
    [SerializeField] private EventTypeDim _onDestroyDimEventSO;

    [Header("RaiseEvents")]
    [SerializeField] private EventTypeVoid _leftGunFireSO;
    [SerializeField] private EventTypeVoid _rightGunFireSO;

    [Space()]
    [SerializeField] private bool _forbiddenAttackBySearchMode = true;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _aimPivot;
    [SerializeField] private Gun[] _leftGuns;
    [SerializeField] private Gun[] _rightGuns;

    public Gun LeftGun { get; private set; }
    public Gun RightGun { get; private set; }

    private bool _isSearchMode;
    private Transform _camTr;
    private Transform _aimPivotTr;
    private RaycastHit[] raycastHits = new RaycastHit[64];
    private Rect _cachedRect = new Rect();
    private List<DimObject> _spawnedDims = new List<DimObject>();

    private void Awake()
    {
        Debug.Assert(_normalModeEventSO != null && _searchModeEventSO != null);
        Debug.Assert(_leftGunFireSO != null && _rightGunFireSO != null);
        Debug.Assert(_camera != null && _aimPivot != null);

        _camTr = _camera.transform;
        _aimPivotTr = _aimPivot.transform;

        SetupGuns();
    }

    private void OnEnable()
    {
        _normalModeEventSO.OnEvent += OnNormalMode;
        _searchModeEventSO.OnEvent += OnSearchMode;

        _onSpawnDimEventSO.OnEvent += OnSpawnDim;
        _onDestroyDimEventSO.OnEvent += OnDestroyDim;

        _leftGunFireSO.OnEvent += FireLeftGun;
        _rightGunFireSO.OnEvent += FireRightGun;
    }

    private void OnDisable()
    {
        _normalModeEventSO.OnEvent -= OnNormalMode;
        _searchModeEventSO.OnEvent -= OnSearchMode;

        _onSpawnDimEventSO.OnEvent -= OnSpawnDim;
        _onDestroyDimEventSO.OnEvent -= OnDestroyDim;

        _leftGunFireSO.OnEvent -= FireLeftGun;
        _rightGunFireSO.OnEvent -= FireRightGun;
    }

    private void SetupGuns()
    {
        if (_leftGuns.Length > 0)
            LeftGun = _leftGuns[0];

        if (_rightGuns.Length > 0)
            RightGun = _rightGuns[0];
    }    

    public void FireLeftGun()
    {
        if (_forbiddenAttackBySearchMode && _isSearchMode)
            return;

        AimAtCenter(LeftGun);
        LeftGun?.Fire();
    }

    public void FireRightGun()
    {
        if (_forbiddenAttackBySearchMode && _isSearchMode)
            return;

        AimAtCenter(RightGun);
        RightGun?.Fire();
    }

    public void FireLeftTrackingGun()
    {
        FireTrackingGun(_leftGuns);
    }

    public void FireRightTrackingGun()
    {
        FireTrackingGun(_rightGuns);
    }

    // 0번 총은 제외함...
    private void FireTrackingGun(Gun[] guns)
    {
        if (!GameManager.Instance.UseTrackingGun())
            return;

        var visibles =
            _spawnedDims.Where(x => x.gameObject.IsVisibleInScreen(_camera, ref _cachedRect)).ToArray();

        if (visibles.Length == 0)
            return;

        for (int i = 1, dimIdx = 0; i < visibles.Length && dimIdx < visibles.Length; ++i)
        {
            while (dimIdx < visibles.Length
                && guns[i].CurrentColor != visibles[dimIdx].CurrentColor)
            {
                ++dimIdx;
            }

            if(dimIdx < visibles.Length)
                guns[i].Fire(visibles[dimIdx++].transform.position);
        }
    }

    private void AimAtCenter(Gun gun)
    {
        if (Physics.RaycastNonAlloc(_camTr.position, _camTr.forward, raycastHits) > 0)
            gun.Aim(raycastHits[0].transform.position);
        else
            gun.Aim(_aimPivotTr.position);
    }

    private void OnNormalMode() => _isSearchMode = false;
    private void OnSearchMode() => _isSearchMode = true;
    private void OnSpawnDim(DimObject obj) => _spawnedDims.Add(obj);
    private void OnDestroyDim(DimObject obj) => _spawnedDims.Remove(obj);
}

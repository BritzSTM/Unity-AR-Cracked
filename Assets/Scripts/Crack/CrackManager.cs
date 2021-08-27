using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

/// <summary>
/// AR 공간상에 Crack 오브젝트를 생성하는 매니저.
/// 매니저마다 전략이 다를 수 있으므로 여러 매니저가 존재할 수 있다.
/// </summary>
public class CrackManager : MonoBehaviour
{
    private Camera _arCamera;
    private ARPlaneManager _arPlaneManager;

    [Header("Raise events")]
    [SerializeField] private EventTypeCrackManager _onCrackDeployMode;
    [SerializeField] private EventTypeCrackManager _onCrackUndeployMode;
    [SerializeField] private EventTypeCrackManager _onCrackDeployFailed;

    [Header("Recive events")]
    [SerializeField] private EventTypeVoid _onNormalModeSO;
    [SerializeField] private EventTypeVoid _onSearchModeSO;

    [SerializeField] private GameObject[] _crackPrefabs;

    // Crack을 배치하기 위한 최소 PlaneSize
    [SerializeField] private float _distanceBetweenCrack = 0.8f;
    [SerializeField] private Vector2 _minimumPlaneSizeForDeploy = (Vector2.one / 10.0f); // 0.1 * 0.1
    [SerializeField] private float _minimumCrackSize = 0.5f;
    [SerializeField] private float _maximumCrackSize = 0.8f;

    [SerializeField] private float _minimumDeployTime = 15.0f;
    [SerializeField] private float _maximumDeployTime = 25.0f;
    [SerializeField] private float _limitDeployTime = 15.0f;

    private bool _isSearchMode;
    private Dictionary<TrackableId, Crack> _arPlaneToSpawners = new Dictionary<TrackableId, Crack>();
    private List<ARPlane> _arPlanesForDeploy = new List<ARPlane>();
    private List<Vector3> _deployedPositions = new List<Vector3>();

    private float _lastDeployTime = 0.0f;
    private float _randDepolyTime = 0.0f;

    private void Awake()
    {
        _arCamera = FindObjectOfType<Camera>();
        _arPlaneManager = FindObjectOfType<ARPlaneManager>();

        Debug.Assert(_arCamera != null && _arPlaneManager != null);
    }

    private bool _wasDeployState;
    private bool _wasDeployFailed;

    private void Update()
    {   
        if (Time.time < _lastDeployTime + _randDepolyTime)
            return;

        if (!_wasDeployState)
        {
            _onCrackDeployMode.RaiseEvent(this);
            _wasDeployState = true;
        }

        if(Time.time > _lastDeployTime + _randDepolyTime + _limitDeployTime)
        {
            if(!_wasDeployFailed)
                _onCrackDeployFailed.RaiseEvent(this);

            _wasDeployFailed = true;
            return;
        }

        if (!_isSearchMode)
            return;

        //Debug.Log($"[{nameof(CrackManager)}] Try depoly creack");
        if (_arPlanesForDeploy.Count == 0)
            return;

        var visibleList = _arPlanesForDeploy.Where(
            (x) => {
                var screenPos = _arCamera.WorldToScreenPoint(x.transform.position);
                bool inScreen = screenPos.x >= 0.0f && screenPos.x <= Screen.width && screenPos.y >= 0.0f && screenPos.y <= Screen.height;

                return (x.GetComponent<Renderer>().isVisible == true) && inScreen;
                }).ToList();

        //Debug.Log($"[{nameof(CrackManager)}] Visiable ar plane : {visibleList.Count}");
        if (visibleList == null || visibleList.Count == 0)
            return;

        //Debug.Log($"[{nameof(CrackManager)}] Depoly crack");
        ARPlane selectedPlane = null;
        int picked;
        if (_deployedPositions.Count == 0)
        {
            picked = Random.Range(0, visibleList.Count);
            selectedPlane = visibleList[picked];
        }
        else
        {
            var correntPlanes = visibleList.Where(x => {
                bool found = false;
                for (int i = 0; i < _deployedPositions.Count; ++i)
                {
                    var dist = (x.center - _deployedPositions[i]).magnitude;
                    if (dist >= _distanceBetweenCrack)
                    {
                        Debug.Log("Found");
                        found = true;
                        break;
                    }
                }

                return found;
            }).ToList();

            if(correntPlanes != null && correntPlanes.Count > 0)
            {
                picked = Random.Range(0, correntPlanes.Count);
                selectedPlane = correntPlanes[picked];
            }
        }

        if (selectedPlane == null)
            return;

        picked = Random.Range(0, _crackPrefabs.Length);
        var createdCrack = Instantiate(_crackPrefabs[picked], selectedPlane.center, selectedPlane.transform.rotation);
        var selectedSize = Random.Range(_minimumCrackSize, _maximumCrackSize);
        createdCrack.transform.localScale = new Vector3(selectedSize, selectedSize, selectedSize);
        _arPlanesForDeploy.Remove(selectedPlane);
        _deployedPositions.Add(selectedPlane.center);

        _lastDeployTime = Time.time;
        _randDepolyTime = Random.Range(_minimumDeployTime, _maximumDeployTime);
        _wasDeployState = false;
        _wasDeployFailed = false;

        _onCrackUndeployMode.RaiseEvent(this);
    }

    private void OnEnable()
    {
        _arPlaneManager.planesChanged += OnChangedARPlane;
        _onNormalModeSO.OnEvent += OnNormalMode;
        _onSearchModeSO.OnEvent += OnSearchMode;
    }

    private void OnDisable()
    {
        _arPlaneManager.planesChanged -= OnChangedARPlane;
        _onNormalModeSO.OnEvent -= OnNormalMode;
        _onSearchModeSO.OnEvent -= OnSearchMode;
    }

    private void OnChangedARPlane(ARPlanesChangedEventArgs arg)
    {
        if(arg.added != null)
        {
            var added = arg.added;
            if(added.Count > 0)
            {
                foreach(var plane in added)
                {
                    if (plane.extents.x * plane.extents.y >= _minimumPlaneSizeForDeploy.x * _minimumPlaneSizeForDeploy.y)
                    {
                        Debug.Log($"[{nameof(CrackManager)}] Add _arPlanesForDeploy");
                        _arPlanesForDeploy.Add(plane);
                    }
                }
            }
        }
    }

    private void OnSearchMode() => _isSearchMode = true;
    private void OnNormalMode() => _isSearchMode = false;
}

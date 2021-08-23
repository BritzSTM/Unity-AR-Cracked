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

    //[SerializeField] private CrackDeployStrategySO _deployStrategySO;
    //private ICrackDeployStrategy _deployStrategy = null;


    [SerializeField] private GameObject _testPrefab;
    [SerializeField] private AssetReference[] _crackPlane;
    [SerializeField] private TMP_Text _text;

    // Crack을 배치하기 위한 최소 PlaneSize
    [SerializeField] private Vector2 _minimumPlaneSizeForDeploy = (Vector2.one / 10.0f); // 0.1 * 0.1
    [SerializeField] private float _minimumCrackSize = 0.1f;
    [SerializeField] private float _maximumCrackSize = 0.4f;

    [SerializeField] private float _minimumDeployTime = 15.0f;
    [SerializeField] private float _maximumDeployTime = 25.0f;

    private Dictionary<TrackableId, Crack> _arPlaneToSpawners = new Dictionary<TrackableId, Crack>();
    private List<ARPlane> _arPlanesForDeploy = new List<ARPlane>();
    private float _lastDeployTime = 0.0f;
    private float _randDepolyTime = 0.0f;

    private int count = 0;

    private void Awake()
    {
        InitARDependency();
        InitSOModules();
    }

    private void Update()
    {   
        if (Time.time < _lastDeployTime + _randDepolyTime)
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
        if (visibleList.Count == 0)
            return;

        //Debug.Log($"[{nameof(CrackManager)}] Depoly crack");
        int picked = Random.Range(0, visibleList.Count);
        var selectedPlane = visibleList[picked];

        //var createdCrack = Addressables.InstantiateAsync(_crackPlane, selectedPlane.center, selectedPlane.transform.rotation);
        var createdCrack = Instantiate(_testPrefab, selectedPlane.center, selectedPlane.transform.rotation);
        var selectedSize = Random.Range(_minimumCrackSize, _maximumCrackSize);
        createdCrack.transform.localScale = new Vector3(selectedSize, selectedSize, selectedSize);

        //createdCrack.Completed += (x) => {
        //    var selectedSize = Random.Range(_minimumCrackSize, _maximumCrackSize);
        //    x.Result.transform.localScale = new Vector3(selectedSize, selectedSize, selectedSize);
        //    Debug.Log("Succed deploy");
        //};

        count++;
        _text.text = count.ToString();

        _lastDeployTime = Time.time;
        _randDepolyTime = Random.Range(_minimumDeployTime, _maximumDeployTime);
    }

    private void OnEnable()
    {
        _arPlaneManager.planesChanged += OnChangedARPlane;
    }

    private void OnDisable()
    {
        _arPlaneManager.planesChanged -= OnChangedARPlane;
    }

    private void InitARDependency()
    {
        _arCamera = FindObjectOfType<Camera>();
        _arPlaneManager = FindObjectOfType<ARPlaneManager>();

        Debug.Assert(_arCamera != null && _arPlaneManager != null);
    }

    private void InitSOModules()
    {

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
}

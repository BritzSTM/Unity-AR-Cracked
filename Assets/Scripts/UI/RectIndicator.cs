using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectIndicator : MonoBehaviour
{
    [SerializeField] private EventTypeGameObject _trackingRequestSO;
    [SerializeField] private EventTypeGameObject _untrackingRequestSO;

    [SerializeField] private Camera _pivotCamera;
    [SerializeField] private GameObject _simbolPrefab;
    [SerializeField] private int _initPoolSize = 20;

    private List<RectTransform> _indicators = new List<RectTransform>();
    private List<GameObject> _trackingObjects = new List<GameObject>();
    private RectTransform _tr;

    private void Awake()
    {
        Debug.Assert(_trackingRequestSO != null && _untrackingRequestSO != null);
        Debug.Assert(_simbolPrefab != null);
        Debug.Assert(_pivotCamera != null);

        _tr = GetComponent<RectTransform>();

        CreateIndicatorsInPool(_initPoolSize);
    }

    private Vector2[] _cachedVector2 = new Vector2[8];
    private Rect _cachedRect = new Rect();
    private void Update()
    {
        // VR 카메라가 계속 이동하므로 항상 갱신하도록 한다.
        for(int i = 0, pickedIndicator = 0; i < _trackingObjects.Count; ++i)
        {
            if (!_trackingObjects[i].IsVisibleInScreen(_pivotCamera, ref _cachedRect))
                continue;

            _cachedVector2[0].x = _cachedRect.xMin;
            _cachedVector2[0].y = _cachedRect.yMin; 

            _cachedVector2[1].x = _cachedVector2[1].y = Mathf.Max(_cachedRect.width, _cachedRect.height);

            _indicators[pickedIndicator].position = _cachedVector2[0];
            _indicators[pickedIndicator].sizeDelta = _cachedVector2[1];
            ++pickedIndicator;

            //_indicators[i].gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        _trackingRequestSO.OnEvent += OnTrackingRequest;
        _untrackingRequestSO.OnEvent += OnUntrackingRequest;
    }

    private void OnDisable()
    {
        _trackingRequestSO.OnEvent -= OnTrackingRequest;
        _untrackingRequestSO.OnEvent -= OnUntrackingRequest;
    }

    private void CreateIndicatorsInPool(int count)
    {
        Vector3 initPos = new Vector3(-10000, 0, 0);

        for (int i = 0; i < count; ++i)
        {
            CreateIndicatorInPool(initPos);
        }
    }

    private void CreateIndicatorInPool(Vector3 initPos)
    {
        var simbol = Instantiate(_simbolPrefab, initPos, Quaternion.identity, _tr);

        _indicators.Add(simbol.GetComponent<RectTransform>());
        //simbol.SetActive(false);
    }

    private void OnTrackingRequest(GameObject obj)
    {
        if (_trackingObjects.Find(x => x == obj) != null)
            return;

        _trackingObjects.Add(obj);

        if(_trackingObjects.Count > _indicators.Count)
        {
            int newCount = (int)((float)_trackingObjects.Count * 0.5f);
            CreateIndicatorsInPool(newCount);
        }
    }

    private void OnUntrackingRequest(GameObject obj)
    {
        var found = _trackingObjects.Find(x => x == obj);

        if (found != null)
            _trackingObjects.Remove(found);
    }
}

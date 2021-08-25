using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.AddressableAssets;

public class Crack : MonoBehaviour
{
    [Header("RaiseEvents")]
    [SerializeField] private EventTypeGameObject _trackingRequestEventSO;
    [SerializeField] private EventTypeGameObject _untrackingRequestEventSO;

    private void Awake()
    {
        Debug.Assert(_trackingRequestEventSO != null & _untrackingRequestEventSO != null);
    }

    private void OnEnable()
    {
        _trackingRequestEventSO.RaiseEvent(gameObject);
    }

    private void OnDisable()
    {
        _untrackingRequestEventSO.RaiseEvent(gameObject);
    }
}

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
    [SerializeField] private EventTypeCrack _OnBurstEventSO;
    [SerializeField] private EventTypeCrack _OnUnburstEventSO;

    [Header("Deciders")]
    [SerializeField] private CrackEmissionDeciderSO[] DeciderSOs;
    private ICrackEmissionDecider[] _deciders;

    [Header("Spawn desc")]
    [SerializeField] private GameObject[] _dimObjects;
    public float MinSpawnTime = 4.5f;
    public float MaxSpawnTime = 7.0f;
    public float EmissionForce = 4.0f;

    private float _pickedSpawnTime;
    private float _lastSpawnTime;
    private Transform _tr;

    [Header("burst desc")]
    [Range(0.0f, 1.0f)] public float BurstablePT = 0.35f;
    public float MinBurstTime = 4.5f;
    public float MaxBurstTime = 7.0f;
    public float BurstEmissionForce = 4.0f;
    private float _pickednBurstTime;
    private float _lastnBurstTime;

    private void Awake()
    {
        Debug.Assert(_trackingRequestEventSO != null & _untrackingRequestEventSO != null);
        _tr = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        _trackingRequestEventSO.RaiseEvent(gameObject);
    }

    private void OnDisable()
    {
        _untrackingRequestEventSO.RaiseEvent(gameObject);
    }

    private Vector3[] _cahcedVector3 = new Vector3[8];
    private void Update()
    {
        if (!IsSpawanableTime())
            return;

        int pickedDIm = Random.Range(0, _dimObjects.Length);
        var createdDim = Instantiate(_dimObjects[pickedDIm], _tr.position, Quaternion.identity);
        var rbody = createdDim.GetComponent<Rigidbody>();

        // 발사할 각도 조정
        float half = _tr.eulerAngles.x / 2.0f;
        _cahcedVector3[0].x = Random.Range(-half, half);
        _cahcedVector3[0].y = _tr.eulerAngles.y;
        half = _tr.eulerAngles.z / 2.0f;
        _cahcedVector3[0].z = Random.Range(-half, half);

        var prevRot = _tr.rotation;
        _tr.rotation = Quaternion.LookRotation(_cahcedVector3[0]);
        rbody.AddForce(_tr.forward * EmissionForce);
        _tr.rotation = prevRot;

        _lastSpawnTime = Time.time;
        _pickedSpawnTime = Random.Range(MinSpawnTime, MaxSpawnTime);
    }

    private bool IsSpawanableTime() => Time.time > _lastSpawnTime + _pickedSpawnTime;
}

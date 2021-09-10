using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _autoStart = false;

    [Header("Recive events")]
    [SerializeField] private EventTypeDim _onSpawnDimEventSO;
    [SerializeField] private EventTypeDim _onDestroyDimEventSO;
    [SerializeField] private EventTypeCrackManager _onCrackDeployFailedEventSO;

    [Header("Raise events")]
    [SerializeField] private EventTypeGameManager _onUpdateDimCountEventSO;
    [SerializeField] private EventTypeGameManager _onUpdateMinedCountEventSO;
    [SerializeField] private EventTypeGameManager _onUpdateTimeEventSO;
    [SerializeField] private EventTypeGameManager _onGameoverEventSO;   

    public int MinedDimCount { get; private set; }
    private bool _dirtyMinedDimCount;

    public int CurrentDimCount { get; private set; }
    private bool _dirtyCurrentDimCount;

    public float PlayTime { get; private set; }
    private float _startPlayTime;
    private bool _isPlay;
    private bool _crackDeployFailed;

    [Header("GameRule")]
    public int LimitDimCount = 16;
    public float LimitDimWarningRate = 0.8f;
    public int MultiMineGunCost = 30;

    public static GameManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Application.targetFrameRate = 60;
        }
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        _onSpawnDimEventSO.OnEvent += OnSpawnDimEvent;
        _onDestroyDimEventSO.OnEvent += OnDestroyDimEvent;
        _onCrackDeployFailedEventSO.OnEvent += OnCrackDeployFailedEvent;
    }

    private void OnDisable()
    {
        _onSpawnDimEventSO.OnEvent -= OnSpawnDimEvent;
        _onDestroyDimEventSO.OnEvent -= OnDestroyDimEvent;
        _onCrackDeployFailedEventSO.OnEvent -= OnCrackDeployFailedEvent;
    }

    private void Start()
    {
        if (_autoStart)
            Play();
    }

    private void FixedUpdate()
    {
        if (!_isPlay)
            return;

        UpdatePlayTime();
        UpdateCounts();
    }

    private void Update()
    {
        if (!_isPlay)
            return;

        if (CurrentDimCount > LimitDimCount || _crackDeployFailed)
        {
            _isPlay = false;
            _onGameoverEventSO.RaiseEvent(this);
        }
    }

    private void OnSpawnDimEvent(DimObject dim)
    {
        ++CurrentDimCount;
        _dirtyCurrentDimCount = true;
    }

    private void OnDestroyDimEvent(DimObject dim)
    {
        --CurrentDimCount;
        ++MinedDimCount;

        _dirtyMinedDimCount = true;
        _dirtyCurrentDimCount = true;
    }

    private void OnCrackDeployFailedEvent(CrackManager manager) => _crackDeployFailed = true;

    public void Play()
    {
        _isPlay = true;
        _startPlayTime = Time.time;
    }

    private void UpdatePlayTime()
    {
        PlayTime += Time.deltaTime;
        _onUpdateTimeEventSO.RaiseEvent(this);
    }

    private void UpdateCounts()
    {
        if(_dirtyMinedDimCount)
        {
            _onUpdateDimCountEventSO.RaiseEvent(this);
            _onUpdateMinedCountEventSO.RaiseEvent(this);
        }

        if(!_dirtyMinedDimCount && _dirtyCurrentDimCount)
            _onUpdateDimCountEventSO.RaiseEvent(this);

        _dirtyMinedDimCount = false;
        _dirtyCurrentDimCount = false;
    }

    public bool UseTrackingGun()
    {
        if (MinedDimCount < MultiMineGunCost)
            return false;

        MinedDimCount -= MultiMineGunCost;
        _dirtyMinedDimCount = true;

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Recive events")]
    [SerializeField] private EventTypeDim _onSpawnDimEventSO;
    [SerializeField] private EventTypeDim _onDestroyDimEventSO;

    private int _minedDimCount;
    private int _currentDimCount;

    [Header("UI... Remove it")]
    [SerializeField] private TMP_Text _minedDimText;
    [SerializeField] private TMP_Text _currentDimText;
    [SerializeField] private WarningAlarm _limitDimAlarm;

    [Header("GameRule")]
    public int LimitDimCount = 16;
    public float LimitDimWarningRate = 0.8f;

    public static GameManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        _onSpawnDimEventSO.OnEvent += OnSpawnDimEvent;
        _onDestroyDimEventSO.OnEvent += OnDestroyDimEvent;
    }

    private void OnDisable()
    {
        _onSpawnDimEventSO.OnEvent -= OnSpawnDimEvent;
        _onDestroyDimEventSO.OnEvent -= OnDestroyDimEvent;
    }

    private void Update()
    {
        // warning...
        float currentDimRate = _currentDimCount / LimitDimCount;
        if (currentDimRate >= 1.0f)
        {
            // GameOver
        }

        if(currentDimRate >= 0.8f)
        {
            _limitDimAlarm.ActiveAlarm = true;
        }
        else
        {
            _limitDimAlarm.ActiveAlarm = false;
        }
    }

    private void LateUpdate()
    {
        // Mined
        _minedDimText.text = string.Format("Mined : {0:D8}", _minedDimCount);

        // DimCount
        _currentDimText.text = string.Format("DimCount : {0:D8}", _currentDimCount);
    }

    private void OnSpawnDimEvent(DimObject dim)
    {
        ++_currentDimCount;
    }

    private void OnDestroyDimEvent(DimObject dim)
    {
        --_currentDimCount;
        ++_minedDimCount;
    }
}

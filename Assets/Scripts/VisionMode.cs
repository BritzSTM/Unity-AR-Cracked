using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Volume))]
public class VisionMode : MonoBehaviour
{
    [System.Serializable]
    public enum Mode
    {
        Normal,
        Search
    }

    [Header("PP")]
    [SerializeField] private VolumeProfile _normalModePP;
    [SerializeField] private VolumeProfile _searchModePP;
    [SerializeField] private Mode _initVision;

    [Header("SoundFX")]
    [SerializeField] private AudioClip _searchSoundFX;

    [Header("ReciveEvents")]
    [SerializeField] private EventTypeVoid _zoomInEventSO;
    [SerializeField] private EventTypeVoid _zoomOutEventSO;

    [Header("RaiseEvents")]
    [SerializeField] private EventTypeVoid _normalModeEventSO;
    [SerializeField] private EventTypeVoid _searchModeEventSO;

    public Mode CurrentMode { get; private set; }

    private Volume _ppVolume;
    private AudioSource _audioSource;
    private ARPlaneManager _arPlaneManager;
    
    private void Awake()
    {
        _ppVolume = GetComponent<Volume>();
        Debug.Assert(_normalModePP != null && _searchModePP != null);
        Debug.Assert(_ppVolume != null);

        _audioSource = GetComponent<AudioSource>();
        Debug.Assert(_searchSoundFX != null);
        Debug.Assert(_audioSource != null);

        Debug.Assert(_zoomInEventSO != null && _zoomOutEventSO != null);
        Debug.Assert(_normalModeEventSO != null && _searchModeEventSO != null);

        _arPlaneManager = FindObjectOfType<ARPlaneManager>();
        Debug.Assert(_arPlaneManager != null);

        ChangeVision(_initVision);
    }

    private void OnEnable()
    {
        _zoomInEventSO.OnEvent += ChangeSearchVision;
        _zoomOutEventSO.OnEvent += ChangeNormalVision;
    }

    private void OnDisable()
    {
        _zoomInEventSO.OnEvent -= ChangeSearchVision;
        _zoomOutEventSO.OnEvent -= ChangeNormalVision;
    }

    public void ChangeVision(Mode mode)
    {
        if (mode == Mode.Normal)
        {
            _ppVolume.profile = _normalModePP;
            _audioSource.Stop();
            _arPlaneManager.enabled = false;
            _normalModeEventSO.RaiseEvent();
        }
        else if (mode == Mode.Search)
        {
            if (CurrentMode == Mode.Search)
                return;

            _ppVolume.profile = _searchModePP;
            _audioSource.clip = _searchSoundFX;
            _audioSource.Play();
            _arPlaneManager.enabled = true;
            _searchModeEventSO.RaiseEvent();
        }

        CurrentMode = mode;
    }

    public void ChangeVision(int mode)
    {
        ChangeVision((Mode)mode);
    }

    public void ChangeNormalVision()
    {
        ChangeVision(Mode.Normal);
    }

    public void ChangeSearchVision()
    {
        ChangeVision(Mode.Search);
    }
}

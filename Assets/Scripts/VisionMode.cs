using UnityEngine;
using UnityEngine.Rendering;

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

    public Mode CurrentMode { get; private set; }

    private Volume _ppVolume;
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _ppVolume = GetComponent<Volume>();
        Debug.Assert(_normalModePP != null && _searchModePP != null);
        Debug.Assert(_ppVolume != null);

        _audioSource = GetComponent<AudioSource>();
        Debug.Assert(_searchSoundFX != null);
        Debug.Assert(_audioSource != null);

        Debug.Assert(_zoomInEventSO != null && _zoomOutEventSO != null);
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
        }
        else if (mode == Mode.Search)
        {
            if (CurrentMode == Mode.Search)
                return;

            _ppVolume.profile = _searchModePP;
            _audioSource.clip = _searchSoundFX;
            _audioSource.Play();
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

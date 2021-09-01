using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DimObject : MonoBehaviour, IDamageable
{
    public Color CurrentColor { get; private set; }
    private Transform _tr;
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private ARCameraManager _camera;
    private ParticleSystem _brokenVisualEffect;
    private MaterialPropertyBlock _mpb;
    private float _playDeployAnimeTime;
    private float _lastPlayRate;

    [Header("RaiseEvents")]
    [SerializeField] private EventTypeGameObject _trackingRequestEventSO;
    [SerializeField] private EventTypeGameObject _untrackingRequestEventSO;
    [SerializeField] private EventTypeDim _onSpawnDimEventSO;
    [SerializeField] private EventTypeDim _onDestroyDimEventSO;
    [SerializeField] private EventTypeAudioClip _playEffectSoundSO;

    [Header("Matrial")]
    [SerializeField] private Material[] _materials;
    [SerializeField] private bool _IsPlayDeployAnime = true;
    [SerializeField] private float _targetDeployAnimeTime = 1.0f;

    [Header("FXs")]
    [SerializeField] private AudioClip[] _brokenSoundEffects;
    [SerializeField] private ParticleSystem _brokenVisualEffectPrefab;

    // 이 항목은 객체가 완전히 제거될 떄의 지연시간을 의미합니다.
    [SerializeField] private float _destroyDelay = 2.0f;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _camera = FindObjectOfType<ARCameraManager>();
        _mpb = new MaterialPropertyBlock();

        InitFXs();
    }

    private void OnEnable()
    {
        _playDeployAnimeTime = 0.0f;
        _lastPlayRate = 0.0f;

        _onSpawnDimEventSO.RaiseEvent(this);
    }

    private void OnDisable()
    {
        _untrackingRequestEventSO.RaiseEvent(gameObject);
        _onDestroyDimEventSO.RaiseEvent(this);
    }

    private void Start()
    {
        _trackingRequestEventSO.RaiseEvent(gameObject);
        ChangeRandomFormMats();
    }

    private void LateUpdate()
    {
        AnimeDepoly();
    }

    private void ChangeColorFromMats(Material mat)
    {
        _meshRenderer.sharedMaterial = mat;
        CurrentColor = mat.color;
    }

    private void ChangeRandomFormMats()
    {
        var pos = Random.Range(0, _materials.Length);
        ChangeColorFromMats(_materials[pos]);
    }

    public void OnDamage(IDamageable.DamageType type)
    {
        if (_lastPlayRate < 1.0f)
            return;

        if (type.color == CurrentColor)
        {
            PlayDestroyVisualFX();
            PlayDestroySoundFXRandomly();

            Destroy(gameObject, _destroyDelay);
        }
    }

    private void AnimeDepoly()
    {
        if (!_IsPlayDeployAnime && _lastPlayRate >= 1.0f)
            return;

        _playDeployAnimeTime += Time.deltaTime;

        _lastPlayRate = _playDeployAnimeTime / _targetDeployAnimeTime;
        if (_lastPlayRate < 1.0f)
        {
            _mpb.SetFloat(Shader.PropertyToID("_DeployRate"), _lastPlayRate);
            _meshRenderer.SetPropertyBlock(_mpb);
        }
        else
            _meshRenderer.SetPropertyBlock(null);
    }

    private void InitFXs()
    {
        if (_brokenVisualEffectPrefab == null)
        {
            Debug.LogWarning("BrokenVisualEffect null");
            return;
        }

        _brokenVisualEffect = Instantiate(_brokenVisualEffectPrefab, Vector3.zero, Quaternion.identity, _tr);
        _brokenVisualEffect.transform.localPosition = Vector3.zero;
        _brokenVisualEffect.transform.localScale = Vector3.one;
    }

    private void PlayDestroySoundFXRandomly()
    {
        if (_brokenSoundEffects.Length == 0)
        {
            Debug.LogWarning($"[{nameof(DimObject)}] Broken sound effect fx empty");
            return;
        }

        if (_playEffectSoundSO == null)
        {
            Debug.LogWarning($"[{nameof(DimObject)}] effect sound play so null");
            return;
        }

        int picked = Random.Range(0, _brokenSoundEffects.Length);
        _playEffectSoundSO.RaiseEvent(_brokenSoundEffects[picked]);
    }

    private void PlayDestroyVisualFX()
    {
        _meshRenderer.enabled = false;
        _collider.enabled = false;

        _brokenVisualEffect.Play();
    }
}

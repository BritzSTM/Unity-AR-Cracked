using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DimObject : MonoBehaviour, IDamageable
{
    public Color CurrentColor { get; private set; }
    
    public Vector3 Velocity;
    private Transform _tr;
    private MeshRenderer _meshRenderer;
    private ARCameraManager _camera;
    public Camera _some;

    [Header("RaiseEvents")]
    [SerializeField] private EventTypeGameObject _trackingRequestEventSO;
    [SerializeField] private EventTypeGameObject _untrackingRequestEventSO;
    [SerializeField] private EventTypeDim _OnSpawnDimEventSO;
    [SerializeField] private EventTypeDim _OnDestroyDimEventSO;

    [Header("Matrial")]
    [SerializeField] private Material[] _materials;
    [SerializeField] private bool _IsPlayDeployAnime = true;
    [SerializeField] private float _targetDeployAnimeTime = 1.0f;

    private float _playDeployAnimeTime;
    private float _lastPlayRate;
    private MaterialPropertyBlock _mpb;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _camera = FindObjectOfType<ARCameraManager>();
        _mpb = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        _playDeployAnimeTime = 0.0f;
        _lastPlayRate = 0.0f;

        _OnSpawnDimEventSO.RaiseEvent(this);
    }

    private void OnDisable()
    {
        _untrackingRequestEventSO.RaiseEvent(gameObject);
        _OnDestroyDimEventSO.RaiseEvent(this);
    }

    private void Start()
    {
        _trackingRequestEventSO.RaiseEvent(gameObject);
        ChangeRandomFormMats();
    }

    private void Update()
    {
        if (Velocity.magnitude != 0)
            _tr.Translate(Velocity * Time.deltaTime);
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
        if(type.color == CurrentColor)
            Destroy(gameObject);
    }

    private void AnimeDepoly()
    {
        if (!_IsPlayDeployAnime && _lastPlayRate >= 1.0f)
            return;

        _playDeployAnimeTime += Time.deltaTime;

        _lastPlayRate = _playDeployAnimeTime / _targetDeployAnimeTime;
        if(_lastPlayRate < 1.0f)
        {
            _mpb.SetFloat(Shader.PropertyToID("_DeployRate"), _lastPlayRate);
            _meshRenderer.SetPropertyBlock(_mpb);
        }
        else
            _meshRenderer.SetPropertyBlock(null);
    }
}

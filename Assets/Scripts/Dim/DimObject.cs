using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.AddressableAssets;

public class DimObject : MonoBehaviour, IDamageable
{
    [SerializeField] Color[] _colorMap;

    public Color CurrentColor { get; private set; }
    
    public Vector3 Velocity;
    private Transform _tr;
    private MeshRenderer _meshRenderer;
    private ARCameraManager _camera;
    public Camera _some;
    public EventTypeGameObject So;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _camera = FindObjectOfType<ARCameraManager>();
    }

    private void Start()
    {
        So.RaiseEvent(gameObject);
        ChangeRandomFormColorMap();
    }

    private void Update()
    {
        if (Velocity.magnitude != 0)
            _tr.Translate(Velocity * Time.deltaTime);
    }

    private void ChangeColor(Color color)
    {
        _meshRenderer.material.SetColor("_BaseColor", color);
        CurrentColor = color;
    }

    private void ChangeRandomFormColorMap()
    {
        var pos = Random.Range(0, _colorMap.Length);
        ChangeColor(_colorMap[pos]);
    }

    public void OnDamage(IDamageable.DamageType type)
    {
        if(type.color == CurrentColor)
        {
            //Addressables.ReleaseInstance(gameObject);
            Destroy(gameObject);
        }
    }
}

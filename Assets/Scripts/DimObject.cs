using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DimObject : MonoBehaviour, IDamageable
{
    [SerializeField] Color[] _colorMap;

    public Color CurrentColor { get; private set; }
    
    [SerializeField] Vector3 _velocity;
    private Transform _tr;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        ChangeRandomFormColorMap();
    }

    private void Update()
    {
        if (_velocity.magnitude != 0)
            _tr.Translate(_velocity * Time.deltaTime);
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
            Addressables.ReleaseInstance(gameObject);
        }
    }
}

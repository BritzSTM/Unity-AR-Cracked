using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleGuidlineOffset : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _base = 1000.0f;
    private CanvasRenderer _renderer;
    private Material _material;
    private float _currentOffset;

    private void Awake()
    {
        var img = GetComponent<Image>();

        _material = Instantiate(img.material);
        img.material = _material;
    }

    private Vector2 _cachedVec = new Vector2();
    private void LateUpdate()
    {
        _currentOffset += (Time.deltaTime / _base) * _speed;
        _cachedVec.x = _currentOffset;
        _cachedVec.y = 0.0f;

        _material.SetTextureOffset("_MainTex", _cachedVec);

        if (Mathf.Abs(_currentOffset) > 100.0f)
            _currentOffset = 0.0f;
    }
}

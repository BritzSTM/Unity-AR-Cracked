using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPosSetup : MonoBehaviour
{
    [SerializeField] private GameObject _leftGun;
    [SerializeField] private GameObject _rightGun;

    private Camera _camera;
    private void Awake()
    {
        Debug.Assert(_leftGun != null && _rightGun != null);

        _camera = GetComponent<Camera>();
        _leftGun.transform.position = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
        _rightGun.transform.position = _camera.ViewportToWorldPoint(new Vector3(1, 0, _camera.nearClipPlane));
    }
}

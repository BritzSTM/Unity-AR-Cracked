using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotAnime : MonoBehaviour
{
    private Quaternion _quaternion;
    private Transform _tr;

    public float RotSpeed = 10.0f;
    private float _accTime;
    private void Awake()
    {
        _quaternion = Random.rotation;
        _tr = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        _accTime += Time.deltaTime;
        _tr.rotation = Quaternion.AngleAxis(_accTime * RotSpeed, _quaternion.eulerAngles);

        if (_accTime > 360.0f)
            _accTime = 0.0f;
    }
}

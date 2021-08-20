using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] Vector3 _velocity;
    
    private Transform _tr;

    private void Awake()
    {
        _tr = GetComponent<Transform>();
    }

    void Update()
    {
        if(_velocity.magnitude != 0)
            _tr.Translate(_velocity * Time.deltaTime);
    }
}

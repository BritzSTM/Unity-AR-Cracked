using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new dim object pool so", menuName = "Game/Pool/Dim")]
public class DimObjectPoolSO : ScriptableObject
{
    [SerializeField] private int _initPoolSize = 250;
    [SerializeField] private float _growRate = 1.5f;

    [NonSerialized] private List<DimObject> _dimPool = new List<DimObject>();
    [NonSerialized] private List<DimObject> _allocatedDims = new List<DimObject>();

    [NonSerialized] private bool _isInit = false;
    public void Init()
    {

    }

    public DimObject RequestDimObject()
    {
        return null;
    }

    public void ReturnObject(DimObject dim)
    {

    }
}

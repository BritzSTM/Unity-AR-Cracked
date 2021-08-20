using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private ARPlaneManager _arPlaneManager;

    private int count = 0;
    private void OnEnable()
    {
        _arPlaneManager.planesChanged += OnChangedPlane;
    }

    private void OnDisable()
    {
        _arPlaneManager.planesChanged -= OnChangedPlane;
    }

    private void OnChangedPlane(ARPlanesChangedEventArgs arg)
    {
        if (arg.removed.Count > 0)
            ++count;
        
        _text.text = count.ToString();
    }
}

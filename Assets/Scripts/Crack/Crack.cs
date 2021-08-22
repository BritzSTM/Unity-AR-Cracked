using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.AddressableAssets;

public class Crack : MonoBehaviour
{
    [SerializeField] private AssetReference[] _balloons;
    [SerializeField] private Transform[] _spawnPoints;

    void Start()
    {
        StartCoroutine(StartSpawn());
    }

    IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(4.0f);

        for(int i = 0; i < _balloons.Length; ++i)
        {
            var pos = Random.Range(0, 3);
            Addressables.InstantiateAsync(_balloons[i], _spawnPoints[pos].position, Quaternion.identity);
        }

        StartCoroutine(StartSpawn());
    }
}

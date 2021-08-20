using UnityEngine;
using UnityEngine.AddressableAssets;

public class SmokeLife : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 3.0f;

    private float _startTime;
    private void Start()
    {
        _startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - _startTime > _lifeTime)
            Addressables.ReleaseInstance(this.gameObject);
    }
}

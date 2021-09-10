using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EffectFXPlayManager : MonoBehaviour
{
    [SerializeField] private EventTypeAudioClip _requestEventSO;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _requestEventSO.OnEvent += OnPlayEffectFX;
    }

    private void OnDisable()
    {
        _requestEventSO.OnEvent -= OnPlayEffectFX;
    }

    private void OnPlayEffectFX(AudioClip clip)
    {
        if(clip != null)
            _audioSource.PlayOneShot(clip);
    }
}

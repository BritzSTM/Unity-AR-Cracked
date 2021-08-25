using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class WarningAlarm : MonoBehaviour
{
    public bool ActiveAlarm = false;
    [SerializeField] private Image _alarmBG;
    public float BlinkTime = 1.0f;
    private float _accDeltaTime = 0.0f;

    [SerializeField] private AudioClip _alarmSoundFX;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _alarmSoundFX;
        _audioSource.loop = true;

        Debug.Assert(_audioSource != null && _alarmSoundFX != null);
    }

    private void Update()
    {
        if(!ActiveAlarm)
        {
            _alarmBG.color = Color.green;
            _audioSource.Stop();

            return;
        }

        Color color = new Color();
        color.r = 1.0f;
        color.a = Mathf.Lerp(0.0f, 1.0f, Mathf.Sin((_accDeltaTime / BlinkTime) * Mathf.PI));

        _alarmBG.color = color;
        _accDeltaTime += Time.deltaTime;
        if (_accDeltaTime > BlinkTime)
            _accDeltaTime = 0.0f;

        if (!_audioSource.isPlaying)
            _audioSource.Play();
    }
}

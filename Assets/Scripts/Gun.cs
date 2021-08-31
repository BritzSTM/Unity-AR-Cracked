using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class Gun : MonoBehaviour
{
    [SerializeField] private Image _buttonImg;

    [SerializeField] private Color _color;
    [SerializeField] private int _damage = 1;

    [Header("EventSO")]
    [SerializeField] private EventTypeVoid _FireTriggerEventSO;

    [Header("FXs")]
    [SerializeField] bool _useOld = false;
    [SerializeField] private ParticleSystem _ps;
    [SerializeField] private VisualEffect _gunEffect;
    [SerializeField] private AudioClip[] _gunSoundEffects;

    private Transform _tr;
    private AudioSource _audio;
    private RaycastHit _raycastHit = new RaycastHit();

    private void Awake()
    {
        _buttonImg.color = _color;
        
        _tr = GetComponent<Transform>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (_FireTriggerEventSO != null)
            _FireTriggerEventSO.OnEvent += Fire;
    }

    private void OnDisable()
    {
        if (_FireTriggerEventSO != null)
            _FireTriggerEventSO.OnEvent -= Fire;
    }

    public void Fire()
    {
        //if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit))
        //{
        //    if (hit.transform.tag == "Unit")
        //    {
        //        var target = hit.transform.GetComponent<IDamageable>();
        //        if (target != null)
        //            ToDamage(target);
        //    }
        //}

        PlayEffects();
    }

    public void Aim(Vector3 pos) => transform.rotation = Quaternion.LookRotation(pos - _tr.position);

    private void PlayEffects()
    {
        if(_useOld)
            _ps.Play();
        else
            _gunEffect.Play();

        int pickedSound = Random.Range(0, _gunSoundEffects.Length);
        _audio.PlayOneShot(_gunSoundEffects[pickedSound]);
    }

    private void ToDamage(IDamageable target)
    {
        IDamageable.DamageType damage;
        damage.color = _color;
        damage.damage = _damage;

        target.OnDamage(damage);
    }
}

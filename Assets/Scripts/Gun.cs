using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject _cam;
    [SerializeField] private GameObject _aimPivot;

    [SerializeField] private Color _color;
    [SerializeField] private int _damage = 1;
    [SerializeField] private Image _buttonImg;
    [SerializeField] private VisualEffect _gunEffect;
    [SerializeField] private AudioClip[] _gunSoundEffects;

    [SerializeField] bool _useOld = false;
    [SerializeField] private ParticleSystem _ps;

    private AudioSource _audio;
    private void Awake()
    {
        _buttonImg.color = _color;
        transform.rotation = Quaternion.LookRotation(
            _aimPivot.transform.position - transform.position);

        _audio = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        //RaycastHit hit;
        //if(Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit))
        //{
        //    if (hit.transform.tag == "Unit")
        //    {
        //        var target = hit.transform.GetComponent<IDamageable>();
        //        if (target != null)
        //            ToDamage(target);

        //        _gunEffect.Play();
        //    }
        //}
        PlayEffects();
    }

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

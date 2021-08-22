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
        
    private void Awake()
    {
        _buttonImg.color = _color;
        transform.rotation = Quaternion.LookRotation(
            _aimPivot.transform.position - transform.position);
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

        _gunEffect.Play();
    }

    private void ToDamage(IDamageable target)
    {
        IDamageable.DamageType damage;
        damage.color = _color;
        damage.damage = _damage;

        target.OnDamage(damage);
    }
}

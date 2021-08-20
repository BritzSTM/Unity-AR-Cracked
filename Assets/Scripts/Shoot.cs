using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private int _damage = 1;
    [SerializeField] private GameObject _cam;
    [SerializeField] private AssetReference _smokeEffect;

    private void Awake()
    {
        var img = GetComponent<Image>();
        img.color = _color;
    }

    public void Fire()
    {
        RaycastHit hit;
        if(Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit))
        {
            if (hit.transform.tag == "Unit")
            {
                var target = hit.transform.GetComponent<IDamageable>();
                if (target != null)
                    ToDamage(target);

                Addressables.InstantiateAsync(_smokeEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    private void ToDamage(IDamageable target)
    {
        IDamageable.DamageType damage;
        damage.color = _color;
        damage.damage = _damage;

        target.OnDamage(damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public struct DamageType
    {
        public Color color;
        public int damage;
    }

    void OnDamage(DamageType type);
}
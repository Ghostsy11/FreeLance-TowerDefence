using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : ObjectHealth
{
    public float DamageEnemy { get; set; }
    public ParticleSystem onHurtEffect;
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
    public override void UpdateHealthBar()
    {
        base.UpdateHealthBar();
    }
    public override void CalcualteTheHealth(Scrollbar barSlider, float objectCurrentHealth, float maxHealth)
    {
        base.CalcualteTheHealth(barSlider, objectCurrentHealth, maxHealth);
    }

    public override void OnHit()
    {
        TakeDamage(DamageEnemy);
    }

}

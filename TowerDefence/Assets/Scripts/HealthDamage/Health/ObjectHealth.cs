using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHealth : MonoBehaviour, IDamageable
{
    [Header("ObjectHealth Setteings")]
    [SerializeField] float objectHealth = 100f;
    [SerializeField] Scrollbar barSlider;


    private float maxHealth = 100f;
    void Start()
    {
        UpdateHealthBar();
    }

    public virtual void TakeDamage(float damage)
    {
        objectHealth -= damage;
        objectHealth = Mathf.Clamp(objectHealth, 0, maxHealth); // Keep health within bounds
        UpdateHealthBar();
    }

    // Update health bar display
    public virtual void UpdateHealthBar()
    {

        CalcualteTheHealth(barSlider, objectHealth, maxHealth);

    }

    public float ReturnedObjectHealth()
    {
        return barSlider.size = objectHealth / maxHealth;
    }

    public float ResetHealth()
    {
        return objectHealth = 100f;
    }

    public float objectCurrentHealth()
    {
        return objectHealth;
    }
    public virtual void CalcualteTheHealth(Scrollbar barSlider, float objectCurrentHealth, float maxHealth)
    {

        if (barSlider != null)
        {
            barSlider.size = objectCurrentHealth / maxHealth;
        }
        else
        {

            Debug.LogError("Scrollbar has not been assigned");
        }
    }

    public virtual void OnHit()
    {
        var damagePlayer = 20f;
        TakeDamage(damagePlayer);
    }
}

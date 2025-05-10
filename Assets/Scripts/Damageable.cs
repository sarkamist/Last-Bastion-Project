using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    public Dictionary<Damage.DamageType, float> resistances;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void TakeDamage(Damage damage) {
        float damageAmount = damage.damageAmount;

        //Apply resistances if any
        if (resistances.ContainsKey(damage.damageType))
        {
            damageAmount *= resistances[damage.damageType];
        }

        if (damageAmount > currentHealth)
        {
            currentHealth = 0f;
            Death();
        }
        else
        {
            currentHealth -= damageAmount;
        }
    }

    void Death() {
        
    }
}

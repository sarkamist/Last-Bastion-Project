using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    #region Properties
    [SerializeField]
    public Faction _faction;
    public Faction Faction
    {
        get => _faction;
        set => _faction = value;
    }

    [SerializeField]
    public float _maxHealth = 100f;
    public float MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    [SerializeField, ReadOnly]
    private float _currentHealth;
    public float CurrentHealth {
        get => _currentHealth;
        private set => _currentHealth = value;
    }
    #endregion

    #region Events
    public struct DamageTakenContext {
        public Damageable origin;
        public Attacker source;
        public DamageType damageType;
        public float damageAmount;

        public DamageTakenContext(Damageable origin, Attacker source, DamageType damageType, float damageAmount)
        {
            this.origin = origin;
            this.source = source;
            this.damageType = damageType;
            this.damageAmount = damageAmount;
        }
    }

    public event Action<DamageTakenContext> DamageTaken;

    public struct DamageableDeathContext
    {
        public Damageable origin;
        public Attacker source;

        public DamageableDeathContext(Damageable origin, Attacker source)
        {
            this.origin = origin;
            this.source = source;
        }
    }

    public event Action<DamageableDeathContext> DamageableDeath;
    #endregion

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(Attacker source, DamageType damageType, float damageAmount) {
        if (damageAmount > CurrentHealth)
        {
            CurrentHealth = 0f;
            Death(source);
        }
        else
        {
            CurrentHealth -= damageAmount;
        }

        DamageTaken?.Invoke(new DamageTakenContext(this, source, damageType, damageAmount));
    }

    void Death(Attacker source) {
        Destroy(gameObject);

        DamageableDeath?.Invoke(new DamageableDeathContext(this, source));
    }
}

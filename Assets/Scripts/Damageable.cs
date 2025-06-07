using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    #region Properties
    [Header("Targeting")]
    [SerializeField]
    public Faction _faction;
    public Faction Faction
    {
        get => _faction;
        set => _faction = value;
    }

    [SerializeField, ReadOnly]
    public Transform _body;
    public Transform Body
    {
        get => _body;
        private set => _body = value;
    }

    [Header("Health")]
    [SerializeField]
    public double _maxHealth = 100f;
    public double MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    [SerializeField, ReadOnly]
    private double _currentHealth;
    public double CurrentHealth {
        get => _currentHealth;
        private set => _currentHealth = value;
    }
    #endregion

    #region Events
    public struct DamageTakenContext {
        public Damageable origin;
        public Attacker source;
        public DamageType damageType;
        public double damageAmount;

        public DamageTakenContext(Damageable origin, Attacker source, DamageType damageType, double damageAmount)
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
        Body = transform.Find("Body");
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(Attacker source, DamageType damageType, double damageAmount) {
        if (damageAmount >= CurrentHealth)
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
        DamageableDeath?.Invoke(new DamageableDeathContext(this, source));
        Destroy(gameObject);
    }
}

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

    [SerializeField]
    private bool _canDie = true;
    public bool CanDie
    {
        get => _canDie;
        private set => _canDie = value;
    }
    #endregion

    #region Events
    public struct DamageTakenContext {
        public bool cancel;
        public Damageable origin;
        public Attacker source;
        public DamageType damageType;
        public float damageAmount;

        public DamageTakenContext(Damageable origin, Attacker source, DamageType damageType, float damageAmount)
        {
            this.cancel = false;
            this.origin = origin;
            this.source = source;
            this.damageType = damageType;
            this.damageAmount = damageAmount;
        }
    }

    public delegate DamageTakenContext PreDamageTakenHandler(DamageTakenContext context);
    public event PreDamageTakenHandler PreDamageTakenEvent;
    public event Action<DamageTakenContext> DamageTakenEvent;

    public struct HealthGainContext
    {
        public bool cancel;
        public Damageable origin;
        public GameObject source;
        public float healthAmount;

        public HealthGainContext(Damageable origin, GameObject source, float healthAmount)
        {
            this.cancel = false;
            this.origin = origin;
            this.source = source;
            this.healthAmount = healthAmount;
        }
    }

    public delegate HealthGainContext HealthGainHandler(HealthGainContext context);
    public event HealthGainHandler HealthGainEvent;

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

    public event Action<DamageableDeathContext> DamageableDeathEvent;
    #endregion

    void Start()
    {
        if (Body == null)
        {
            Body = transform.Find("Body");
        }
        
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(Attacker source, DamageType damageType, float damageAmount) {
        DamageTakenContext context = new DamageTakenContext(this, source, damageType, damageAmount);

        //We allow delegate subscribers to modify the context and pass it to the next subscriber
        if (PreDamageTakenEvent != null) foreach (Delegate d in PreDamageTakenEvent.GetInvocationList())
        {
                context = (DamageTakenContext) d.DynamicInvoke(context);
        }

        if (context.cancel) return;

        if (context.damageAmount >= CurrentHealth)
        {
            CurrentHealth = 0f;
            Death(source);
        }
        else
        {
            CurrentHealth -= context.damageAmount;
        }

        DamageTakenEvent?.Invoke(context);
    }

    public void GainHealth(GameObject source, float healthAmount)
    {
        HealthGainContext context = new HealthGainContext(this, source, healthAmount);

        //We allow delegate subscribers to modify the context and pass it to the next subscriber
        if (HealthGainEvent != null) foreach (Delegate d in HealthGainEvent.GetInvocationList())
            {
                context = (HealthGainContext) d.DynamicInvoke(context);
            }

        if (context.cancel) return;

        if (context.healthAmount > 0f) CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + context.healthAmount);
    }

    void Death(Attacker source) {
        DamageableDeathContext context = new DamageableDeathContext(this, source);

        DamageableDeathEvent?.Invoke(context);
        if (CanDie) Destroy(gameObject);
    }
}

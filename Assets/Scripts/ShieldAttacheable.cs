using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class ShieldAttacheable : Attacheable
{
    #region Properties
    [SerializeField, ReadOnly]
    private Damageable _damageableRef;
    public Damageable DamageableRef
    {
        get => _damageableRef;
        private set => _damageableRef = value;
    }

    [SerializeField]
    private double _regenerationRate = 1f;
    public double RegenerationRate
    {
        get => _regenerationRate;
        private set => _regenerationRate = value;
    }

    [SerializeField]
    private float _shieldCooldown;
    public float ShieldCooldown
    {
        get => _shieldCooldown;
        private set => _shieldCooldown = value;
    }

    [SerializeField, ReadOnly]
    private float _shieldRemainingCooldown;
    public float ShieldRemainingCooldown
    {
        get => _shieldRemainingCooldown;
        private set => _shieldRemainingCooldown = value;
    }
    #endregion

    public void Start()
    {
        Damageable damageable = GetComponent<Damageable>();
        if (damageable != null)
        {
            DamageableRef = damageable;
        }
        else
        {
            DamageableRef = null;
        }

        damageable.DamageableDeathEvent += OnShieldDeplete;
        InvokeRepeating(nameof(HandleRegeneration), 0f, 1f);
    }

    public void Update()
    {
        if (ShieldRemainingCooldown > 0) ShieldRemainingCooldown = Mathf.Max(0f, ShieldRemainingCooldown - Time.deltaTime);
    }

    public override void AttachTo(Attacher attacher) {
        base.AttachTo(attacher);

        Attacher.GetComponent<Damageable>().PreDamageTakenEvent += OnAttacherDamageTaken;
    }

    private void HandleRegeneration()
    {
        DamageableRef.GainHealth(gameObject, RegenerationRate);
    }

    private Damageable.DamageTakenContext OnAttacherDamageTaken(Damageable.DamageTakenContext context) {
        if (context.damageAmount <= 0 || ShieldRemainingCooldown > 0f) return context;

        //Shield absorbs damage instead of its attacher
        if (context.damageAmount <= DamageableRef.CurrentHealth)
        {
            DamageableRef.TakeDamage(context.source, context.damageType, context.damageAmount);
            context.damageAmount = 0;
        }
        else
        {
            context.damageAmount -= DamageableRef.CurrentHealth;
            DamageableRef.TakeDamage(context.source, context.damageType, DamageableRef.CurrentHealth);
        }

        return context;
    }

    private void OnShieldDeplete(Damageable.DamageableDeathContext context)
    {
        ShieldRemainingCooldown = ShieldCooldown;
    }
}

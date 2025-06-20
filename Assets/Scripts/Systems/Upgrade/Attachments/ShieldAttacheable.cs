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
    }

    public void Update()
    {
        if (ShieldRemainingCooldown > 0) ShieldRemainingCooldown = Mathf.Max(0f, ShieldRemainingCooldown - Time.deltaTime);
    }

    public override void Apply(GameObject upgradeable) {
        base.Apply(upgradeable);

        Attacher.GetComponent<Damageable>().PreDamageTakenEvent += OnAttacherDamageTaken;
    }

    private Damageable.DamageTakenContext OnAttacherDamageTaken(Damageable.DamageTakenContext context) {
        if (context.damageData.amount <= 0 || ShieldRemainingCooldown > 0f) return context;

        //Shield absorbs damage instead of its attacher
        if (context.damageData.amount <= DamageableRef.CurrentHealth)
        {
            DamageableRef.TakeDamage(context.source, context.damageData);
            context.damageData.amount = 0;
            context.cancel = true;
        }
        else
        {
            context.damageData.amount -= DamageableRef.CurrentHealth;
            DamageableRef.TakeDamage(context.source, context.damageData);
        }

        return context;
    }

    private void OnShieldDeplete(Damageable.DamageableDeathContext context)
    {
        ShieldRemainingCooldown = ShieldCooldown;
    }
}

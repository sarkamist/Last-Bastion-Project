using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Bounty : MonoBehaviour
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
    private int _bountyAmount = 5;
    public int BountyAmount
    {
        get => _bountyAmount;
        set => _bountyAmount = value;
    }
    #endregion

    void Start()
    {
        Damageable damageable = GetComponent<Damageable>();
        if (damageable != null)
        {
            DamageableRef = damageable;
            DamageableRef.DamageableDeathEvent += OnDamageableDeath;
        }
        else
        {
            DamageableRef = null;
        }
    }

    void OnDamageableDeath(Damageable.DamageableDeathContext context)
    {
        if (BountyAmount > 0 && Allegiance.IsPlayerFaction(context.source.Faction)) PlayerResources.Instance.IncreaseGold(BountyAmount);
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    #region Properties
    [SerializeField]
    private Faction _faction;
    public Faction Faction
    {
        get => _faction;
        set => _faction = value;
    }

    [SerializeField, ReadOnly]
    private Damageable _currentTarget;
    public Damageable CurrentTarget
    {
        get => _currentTarget;
        private set => _currentTarget = value;
    }

    [SerializeField, ReadOnly]
    private Moveable _moveableRef;
    public Moveable MoveableRef
    {
        get => _moveableRef;
        private set => _moveableRef = value;
    }

    [SerializeField]
    private float _range = 5f;
    public float Range
    {
        get => _range;
        set => _range = value;
    }

    [SerializeField]
    private DamageType _damageType = DamageType.Physical;
    public DamageType DamageType
    {
        get => _damageType;
        set => _damageType = value;
    }

    [SerializeField]
    private float _damageAmount = 10f;
    public float DamageAmount
    {
        get => _damageAmount;
        set => _damageAmount = value;
    }

    [SerializeField]
    private float _attackSpeed = 2f;
    public float AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed = value;
    }

    [SerializeField, ReadOnly]
    private float _attackCooldown = 0f;
    public float AttackCooldown
    {
        get => _attackCooldown;
        private set => _attackCooldown = value;
    }
    #endregion

    void Start()
    {
        if (GetComponent<Moveable>() is Moveable moveable)
        {
            MoveableRef = moveable;
        }
        else
        {
            MoveableRef = null;
        }
    }

    void Update()
    {
        AcquireTarget();
        CheckAttackConditions();
    }

    void AcquireTarget() {
        List<Damageable> damageables = FindObjectsByType<Damageable>(FindObjectsSortMode.None).ToList();
        damageables = damageables.Where(x => 
            Allegiance.GetEnemies(Faction).Contains(x.Faction)
            && x.gameObject.GetInstanceID() != gameObject.GetInstanceID()
        ).ToList();

        float shortestDistance = Mathf.Infinity;
        Damageable nearestDamageable = null;

        foreach (Damageable damageable in damageables) {
            float distanceToDamageable = Vector3.Distance(transform.position, damageable.transform.position);
            if (distanceToDamageable < shortestDistance)
            {
                shortestDistance = distanceToDamageable;
                nearestDamageable = damageable;
            }
        }

        if (nearestDamageable != null)
        {
            CurrentTarget = nearestDamageable;
        }
        else {
            CurrentTarget = null;
        }
    }

    void CheckAttackConditions() {
        if (CurrentTarget is null) return;

        float distanceToTarget = Vector3.Distance(transform.position, CurrentTarget.transform.position);
        if (distanceToTarget <= Range) {
            if (MoveableRef.IsMoving) MoveableRef.Stop();

            AttackCooldown -= Time.deltaTime;
            if (AttackCooldown <= 0f)
            {
                AttackCooldown = AttackSpeed;
                Attack();
            }
        }
        else if (MoveableRef is not null)
        {
            MoveableRef.MoveAgainst(CurrentTarget.transform);
        }
    }

    void Attack() {
        CurrentTarget.TakeDamage(this, DamageType, DamageAmount);
    }
}

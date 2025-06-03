using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    #region Properties
    [Header("Targeting")]
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

    [SerializeField]
    private float _acquisitionDelay = 0f;
    public float AcquisitionDelay
    {
        get => _acquisitionDelay;
        set => _acquisitionDelay = value;
    }

    [Header("Movement")]
    [SerializeField, ReadOnly]
    private Moveable _moveableRef;
    public Moveable MoveableRef
    {
        get => _moveableRef;
        private set => _moveableRef = value;
    }

    [Header("Attack")]
    [SerializeField]
    private float _maxRange = 5f;
    public float MaxRange
    {
        get => _maxRange;
        set => _maxRange = value;
    }

    [SerializeField]
    private float _minRange = 5f;
    public float MinRange
    {
        get => _minRange;
        set => _minRange = value;
    }

    [SerializeField]
    private GameObject _projectilePrefab = null;
    public GameObject ProjectilePrefab
    {
        get => _projectilePrefab;
        set => _projectilePrefab = value;
    }

    [SerializeField, ReadOnly]
    public Transform _body;
    public Transform Body
    {
        get => _body;
        private set => _body = value;
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

    [Header("Damage")]
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
    #endregion

    void Start()
    {
        Moveable moveable = GetComponent<Moveable>();
        if (moveable != null)
        {
            MoveableRef = moveable;
        }
        else
        {
            MoveableRef = null;
        }
        Body = transform.Find("Body");
    }

    void Update()
    {
        if (AcquisitionDelay > 0) AcquisitionDelay -= Time.deltaTime;
        if (
            CurrentTarget == null || !IsInRange(CurrentTarget.transform)
            && AcquisitionDelay <= 0f
        )
        {
            AcquireTarget();
        }

        CheckAttackConditions();
    }

    void AcquireTarget()
    {
        List<Damageable> damageables = FindObjectsByType<Damageable>(FindObjectsSortMode.None).ToList();
        damageables = damageables.Where(x =>
            IsEnemyFaction(x.Faction)
            && IsOverMinRange(x.transform)
            && x.gameObject.GetInstanceID() != gameObject.GetInstanceID()
        ).ToList();

        if (damageables.Count > 0)
        {
            AcquisitionDelay = Random.Range(AttackCooldown, AttackCooldown * 1.5f);
            CurrentTarget = damageables[Random.Range(0, damageables.Count)];
        }
        else
        {
            CurrentTarget = null;
        }
    }
    bool IsInRange(Transform target)
    {
        return IsOverMinRange(target) && IsWithinMaxRange(target);
    }

    bool IsOverMinRange(Transform target)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        return (distanceToTarget >= MinRange);
    }

    bool IsWithinMaxRange(Transform target)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        return (distanceToTarget <= MaxRange);
    }

    void CheckAttackConditions()
    {
        if (CurrentTarget == null) return;

        if (IsInRange(CurrentTarget.transform))
        {
            if (MoveableRef != null && !MoveableRef.Agent.isStopped) MoveableRef.DisableMovement();

            if (AttackCooldown > 0f) AttackCooldown -= Time.deltaTime;
            if (AttackCooldown <= 0f)
            {
                AttackCooldown = AttackSpeed;
                Attack();
            }
        }
        else if (MoveableRef != null)
        {
            MoveableRef.MoveTo(CurrentTarget.transform, MaxRange);
        }
    }

    void Attack()
    {
        if (ProjectilePrefab != null && Body != null)
        {
            GameObject projectile = Instantiate(ProjectilePrefab, Body.position, Body.rotation);
            projectile.GetComponent<AttackProjectile>().Configure(this, CurrentTarget, DamageType, DamageAmount);
        }
        else
        {
            CurrentTarget.TakeDamage(this, DamageType, DamageAmount);
        }
    }

    bool IsEnemyFaction(Faction faction)
    {
        return Allegiance.GetEnemies(Faction).Contains(faction);
    }
}

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

    [SerializeField, ReadOnly]
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
        if (CurrentTarget == null || !IsInRange(CurrentTarget.transform))
        {
            if (AcquisitionDelay <= 0f)
            {
                AcquireTarget();
            }
            else {
                AcquisitionDelay -= Time.deltaTime;
            }
        }

        CheckAttackConditions();
    }

    void AcquireTarget() {
        List<Damageable> damageables = FindObjectsByType<Damageable>(FindObjectsSortMode.None).ToList();
        damageables = damageables.Where(x => 
            Allegiance.GetEnemies(Faction).Contains(x.Faction)
            && x.gameObject.GetInstanceID() != gameObject.GetInstanceID()
            && Vector3.Distance(transform.position, x.transform.position) >= MinRange
        ).ToList();

        if (damageables.Count > 0)
        {
            CurrentTarget = damageables[Random.Range(0, damageables.Count)];
            AcquisitionDelay = Random.Range(0f, 2.5f);
        }
        else {
            CurrentTarget = null;
        }
    }

    bool IsInRange(Transform target) {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        return (distanceToTarget <= MaxRange && distanceToTarget >= MinRange);
    }

    void CheckAttackConditions() {
        if (CurrentTarget == null) return;

        if (IsInRange(CurrentTarget.transform)) {
            if (MoveableRef != null && MoveableRef.IsMoving) MoveableRef.Stop();

            AttackCooldown -= Time.deltaTime;
            if (AttackCooldown <= 0f)
            {
                AttackCooldown = AttackSpeed;
                Attack();
            }
        }
        else if (MoveableRef != null)
        {
            MoveableRef.MoveAgainst(CurrentTarget.transform, MaxRange);
        }
    }

    void Attack() {
        if (ProjectilePrefab != null && Body != null)
        {
            GameObject projectile = Instantiate(ProjectilePrefab, Body.position, Body.rotation);
            projectile.GetComponent<AttackProjectile>().Configure(this, CurrentTarget, DamageType, DamageAmount);
        }
        else {
            CurrentTarget.TakeDamage(this, DamageType, DamageAmount);
        }
    }
}

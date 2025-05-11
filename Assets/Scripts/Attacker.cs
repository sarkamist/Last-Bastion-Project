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
    private float _range = 5f;
    public float Range
    {
        get => _range;
        set => _range = value;
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
        if (CurrentTarget == null) return;

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
        else if (MoveableRef != null)
        {
            MoveableRef.MoveAgainst(CurrentTarget.transform);
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

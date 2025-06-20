using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    #region Properties
    [Header("Projectile")]
    [SerializeField]
    private float _travelSpeed = 25f;
    public float TravelSpeed
    {
        get => _travelSpeed;
        set => _travelSpeed = value;
    }

    [SerializeField]
    private float _lifespan = 5f;
    public float Lifespan
    {
        get => _lifespan;
        set => _lifespan = value;
    }

    [Header("Targeting")]
    [SerializeField, ReadOnly]
    private Damageable _currentTarget;
    public Damageable CurrentTarget
    {
        get => _currentTarget;
        private set => _currentTarget = value;
    }

    [Header("Targeting")]
    private Attacker _attackSource;
    public Attacker AttackSource
    {
        get => _attackSource;
        set => _attackSource = value;
    }

    [Header("Damage")]
    [SerializeField]
    private DamageData _damageData;
    public DamageData DamageData
    {
        get => _damageData;
        set => _damageData = value;
    }
    #endregion

    private void Start()
    {
        Destroy(gameObject, Lifespan);
    }

    private void Update()
    {
        if (CurrentTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, CurrentTarget.Body.position, TravelSpeed * Time.deltaTime);
            Vector3 direction = CurrentTarget.Body.position - transform.position;
            float distance = TravelSpeed * Time.deltaTime;

            if (direction.magnitude <= distance)
            {
                HitTarget();
                return;
            }
        }
    }

    private void HitTarget()
    {
        Destroy(gameObject);
        CurrentTarget.TakeDamage(AttackSource, DamageData);
    }

    public void Configure(Attacker attackSource, Damageable currentTarget, DamageData damageData)
    {
        this.AttackSource = attackSource;
        this.CurrentTarget = currentTarget;
        this.DamageData = damageData;
    }
}

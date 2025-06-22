using UnityEngine;

public class DefenderAttacheable : Attacheable
{
    #region Properties
    [Header("Defender Parameters")]
    [SerializeField]
    private GameObject _defenderPrefab;
    public GameObject DefenderPrefab
    {
        get => _defenderPrefab;
        private set => _defenderPrefab = value;
    }

    [SerializeField, ReadOnly]
    private GameObject _currentDefender = null;
    public GameObject CurrentDefender
    {
        get => _currentDefender;
        private set => _currentDefender = value;
    }

    [Header("Spawner Parameters")]
    [SerializeField]
    private float _spawnCooldown = 20f;
    public float SpawnCooldown
    {
        get => _spawnCooldown;
        private set => _spawnCooldown = value;
    }

    [SerializeField, ReadOnly]
    private float _spawnTimer = 0f;
    public float SpawnTimer
    {
        get => _spawnTimer;
        private set => _spawnTimer = value;
    }
    #endregion

    void Start()
    {
        SpawnTimer = 0.25f * SpawnCooldown;
    }

    void Update()
    {
        if (SpawnTimer > 0f)
        {
            SpawnTimer -= Time.deltaTime;
        }
        else if (CurrentDefender == null)
        {
            SpawnDefender();
        }
    }

    void OnDefenderDeath(Damageable.DamageableDeathContext context)
    {
        CurrentDefender = null;
        SpawnTimer = SpawnCooldown;
    }

    void SpawnDefender()
    {
        CurrentDefender = Instantiate(DefenderPrefab, Random.insideUnitSphere * 5f + Attacher.gameObject.transform.position, Random.rotation);
        CurrentDefender.transform.position = new Vector3(CurrentDefender.transform.position.x, Attacher.gameObject.transform.position.y, CurrentDefender.transform.position.z);
        CurrentDefender.GetComponent<Damageable>().DamageableDeathEvent += OnDefenderDeath;
    }
}

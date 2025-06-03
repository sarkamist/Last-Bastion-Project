using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    #region Properties
    [Header("Map Parameters")]
    [SerializeField]
    private Renderer _spawnArea = null;
    public Renderer SpawnArea
    {
        get => _spawnArea;
        set => _spawnArea = value;
    }

    [SerializeField]
    private float _borderOffset = 1f;
    public float BorderOffset
    {
        get => _borderOffset;
        set => _borderOffset = value;
    }

    [Header("Wave Parameters")]
    [SerializeField]
    private List<EnemyProgression> _availableEnemyProgressions;
    public List<EnemyProgression> AvailableEnemyProgressions
    {
        get => _availableEnemyProgressions;
        set => _availableEnemyProgressions = value;
    }

    [SerializeField]
    private int _initialEnemiesAmount = 4;
    public int InitialEnemiesAmount
    {
        get => _initialEnemiesAmount;
        set => _initialEnemiesAmount = value;
    }

    [SerializeField]
    private int _enemyAmountIncrease = 2;
    public int EnemyAmountIncrease
    {
        get => _enemyAmountIncrease;
        set => _enemyAmountIncrease = value;
    }

    [Header("Current Wave Data")]
    [SerializeField, ReadOnly]
    private int _currentEnemiesAmount = 0;
    public int CurrentEnemiesAmount
    {
        get => _currentEnemiesAmount;
        private set => _currentEnemiesAmount = value;
    }

    [SerializeField, ReadOnly]
    private List<EnemyProgression> _currentWaveProgressions;
    public List<EnemyProgression> CurrentWaveProgressions
    {
        get => _currentWaveProgressions;
        private set => _currentWaveProgressions = value;
    }

    [SerializeField, ReadOnly]
    private float _spawnInterval;
    public float SpawnInterval
    {
        get => _spawnInterval;
        private set => _spawnInterval = value;
    }
    #endregion

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        RoundManager.Instance.RoundStart += OnRoundStart;
        RoundManager.Instance.RoundEnd += OnRoundEnd;
    }

    void OnRoundStart(RoundManager roundManager, float roundDuration)
    {
        CurrentWaveProgressions = new List<EnemyProgression>();

        for (int i = 0; i < CurrentEnemiesAmount; i++)
        {
            CurrentWaveProgressions.Add(AvailableEnemyProgressions[Random.Range(0, AvailableEnemyProgressions.Count)]);
        }

        SpawnInterval = roundDuration / CurrentWaveProgressions.Count;

        StartCoroutine(SpawnEnemiesLoop(roundManager.CurrentRound));
    }

    void OnRoundEnd(RoundManager roundManager)
    {
        if (roundManager.CurrentRound == 0)
        {
            CurrentEnemiesAmount = InitialEnemiesAmount;
        }
        else
        {
            CurrentEnemiesAmount += EnemyAmountIncrease;
        }
    }

    IEnumerator SpawnEnemiesLoop(int currentRound)
    {
        for (int i = 0; i < CurrentWaveProgressions.Count; i++)
        {
            if (RoundManager.Instance.IsGameover) yield break;
            SpawnEnemy(i, currentRound);
            yield return new WaitForSeconds(SpawnInterval - Random.Range(0f, SpawnInterval * 0.5f));
        }
    }

    void SpawnEnemy(int progressionIndex, int currentRound)
    {
        EnemyProgression progression = CurrentWaveProgressions[progressionIndex];

        Vector3 spawnPoint = GetRandomEdgePosition();
        progression.InstantiateScaledEnemy(currentRound, spawnPoint, Quaternion.identity);
    }

    Vector3 GetRandomEdgePosition()
    {
        Bounds bounds = SpawnArea.bounds;
        float x, z;

        //Randomly determine one of the four edges of the spawnArea (0 = top, 1 = bottom, 2 = left, 3 = right);
        int edge = Random.Range(0, 4);

        //Assign a random x and z postion depending on the selected edge
        switch (edge)
        {
            case 0: //Top
                x = Random.Range(bounds.min.x + BorderOffset, bounds.max.x - BorderOffset);
                z = bounds.max.z - BorderOffset;
                break;
            case 1: //Bottom
                x = Random.Range(bounds.min.x + BorderOffset, bounds.max.x - BorderOffset);
                z = bounds.min.z + BorderOffset;
                break;
            case 2: //Left
                x = bounds.min.x + BorderOffset;
                z = Random.Range(bounds.min.z + BorderOffset, bounds.max.z - BorderOffset);
                break;
            case 3: //Right
                x = bounds.max.x - BorderOffset;
                z = Random.Range(bounds.min.z + BorderOffset, bounds.max.z - BorderOffset);
                break;
            default:
                x = z = 0; //Fallback
                break;
        }

        float y = SpawnArea.bounds.center.y;

        return new Vector3(x, y, z);
    }
}

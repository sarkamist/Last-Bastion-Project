using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public Renderer spawnArea;
    public float borderOffset;

    public List<GameObject> availableEnemyPrefabs;
    public int initialEnemiesAmount = 10;
    public int enemyAmountIncrease = 2;
    private int currentEnemiesAmount;

    private List<GameObject> currentWaveEnemies;
    private float spawnInterval;

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
        currentEnemiesAmount = initialEnemiesAmount;
        RoundManager.Instance.RoundStart += OnRoundStart;
        RoundManager.Instance.RoundEnd += OnRoundEnd;
    }

    void OnRoundStart(float roundDuration) {
        currentWaveEnemies = new List<GameObject>();

        for (int i = 0; i < currentEnemiesAmount; i++)
        {
            currentWaveEnemies.Add(availableEnemyPrefabs[Random.Range(0, availableEnemyPrefabs.Count)]);
        }

        spawnInterval = roundDuration / currentWaveEnemies.Count;

        StartCoroutine(SpawnEnemiesLoop());
    }

    void OnRoundEnd()
    {
        currentEnemiesAmount += enemyAmountIncrease;
    }

    IEnumerator SpawnEnemiesLoop()
    {
        for (int i = 0; i < currentWaveEnemies.Count; i++)
        {
            SpawnEnemy(i);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy(int index) {
        Vector3 spawnPoint = GetRandomEdgePosition();
        Instantiate(currentWaveEnemies[index], spawnPoint, Quaternion.identity);
    }

    Vector3 GetRandomEdgePosition() {
        Bounds bounds = spawnArea.bounds;
        float x, z;

        //Randomly determine one of the four edges of the spawnArea (0 = top, 1 = bottom, 2 = left, 3 = right);
        int edge = Random.Range(0, 4);

        //Assign a random x and z postion depending on the selected edge
        switch (edge) {
            case 0: //Top
                x = Random.Range(bounds.min.x + borderOffset, bounds.max.x - borderOffset);
                z = bounds.max.z - borderOffset;
                break;
            case 1: //Bottom
                x = Random.Range(bounds.min.x + borderOffset, bounds.max.x - borderOffset);
                z = bounds.min.z + borderOffset;
                break;
            case 2: //Left
                x = bounds.min.x + borderOffset;
                z = Random.Range(bounds.min.z + borderOffset, bounds.max.z - borderOffset);
                break;
            case 3: //Right
                x = bounds.max.x - borderOffset;
                z = Random.Range(bounds.min.z + borderOffset, bounds.max.z - borderOffset);
                break;
            default:
                x = z = 0; //Fallback
                break;
        }

        float y = spawnArea.bounds.center.y;

        return new Vector3(x, y, z);
    }
}

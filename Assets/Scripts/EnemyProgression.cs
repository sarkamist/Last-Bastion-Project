using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyProgression", menuName = "Scriptable Objects/EnemyProgression")]
public class EnemyProgression : ScriptableObject
{
    [SerializeField]
    private GameObject _enemyPrefab;
    public GameObject EnemyPrefab
    {
        get => _enemyPrefab;
        set => _enemyPrefab = value;
    }

    [SerializeField]
    private SerializableDictionary<int, float> _damageProgressionChart;
    public SerializableDictionary<int, float> DamageProgressionChart
    {
        get => _damageProgressionChart;
        set => _damageProgressionChart = value;
    }

    [SerializeField]
    private SerializableDictionary<int, float> _healthProgressionChart;
    public SerializableDictionary<int, float> HealthProgressionChart
    {
        get => _healthProgressionChart;
        set => _healthProgressionChart = value;
    }

    public void InstantiateScaledEnemy(int currentRound, Vector3 spawnPoint, Quaternion quaternion) {
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint, quaternion);
        if (DamageProgressionChart.Count > 0)
        {
            int ltoeIndex = DamageProgressionChart.Keys.Aggregate(0, (acc, round) => {
                return (round <= currentRound) ? round : acc;
            });

            float scalingFactor = 1.0f;
            if (DamageProgressionChart.ContainsKey(ltoeIndex))
                scalingFactor = DamageProgressionChart[ltoeIndex];
            newEnemy.GetComponent<Attacker>().DamageAmount *= scalingFactor;

            Debug.Log($"Round {currentRound} matched by index {ltoeIndex}: Damage Scaling Factor is {scalingFactor}");
        }
        if (HealthProgressionChart.Count > 0)
        {
            int ltoeIndex = HealthProgressionChart.Keys.Aggregate(0, (acc, round) => {
                return (round <= currentRound) ? round : acc;
            });

            float scalingFactor = 1.0f;
            if (HealthProgressionChart.ContainsKey(ltoeIndex))
                scalingFactor = HealthProgressionChart[ltoeIndex];
            newEnemy.GetComponent<Damageable>().MaxHealth *= scalingFactor;

            Debug.Log($"Round {currentRound} matched by index {ltoeIndex}: Health Scaling Factor is {scalingFactor}");
        }
    }
}

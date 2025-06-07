using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    private SerializableDictionary<string, string> _progressionChart;
    public SerializableDictionary<string, string> progressionChart
    {
        get => _progressionChart;
        set => _progressionChart = value;
    }
    public void InstantiateScaledEnemy(int currentRound, Vector3 spawnPoint, Quaternion quaternion) {
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPoint, quaternion);

        if (progressionChart.Count < 1) return;

        //Process progression chart values
        foreach (KeyValuePair<string, string> pair in progressionChart)
        {
            //Process damage modifiers definition
            double? dmgIncreaseMod = null;
            Match dmgMatch = Regex.Match(pair.Value, @"(dmg):([.?\d]+)", RegexOptions.IgnoreCase);
            if (dmgMatch.Success)
            {
                dmgIncreaseMod = double.Parse(dmgMatch.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);
            }

            //Process health modifiers definition
            double? hpIncreaseMod = null;
            Match hpMatch = Regex.Match(pair.Value, @"(hp):([.?\d]+)", RegexOptions.IgnoreCase);
            if (hpMatch.Success)
            {
                hpIncreaseMod = double.Parse(hpMatch.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);
            }

            //Process round definition
            foreach (Match match in Regex.Matches(pair.Key, @"(round):(\d+[\+\*]{0,1})", RegexOptions.IgnoreCase))
            {
                int applyInstances = 0;
                string definition = match.Groups[2].Value;
                int targetRound = int.Parse(definition.Replace("+", string.Empty).Replace("*", string.Empty));

                if (definition.EndsWith("*") && currentRound >= targetRound) applyInstances = (currentRound - targetRound) + 1;
                else if (definition.EndsWith("+") && currentRound >= targetRound) applyInstances = 1;
                else if (currentRound == targetRound) applyInstances = 1;

                if (applyInstances > 0)
                {
                    if (dmgIncreaseMod.HasValue)
                    {
                        newEnemy.GetComponent<Attacker>().DamageAmount *= (1 + (dmgIncreaseMod.Value * applyInstances));
                    }
                    if (hpIncreaseMod.HasValue)
                    {
                        newEnemy.GetComponent<Damageable>().MaxHealth *= (1 + (hpIncreaseMod.Value * applyInstances));
                    }
                }
            }
        }

        newEnemy.GetComponent<Attacker>().DamageAmount = Math.Ceiling(newEnemy.GetComponent<Attacker>().DamageAmount);
        newEnemy.GetComponent<Damageable>().MaxHealth = Math.Ceiling(newEnemy.GetComponent<Damageable>().MaxHealth);
        //Debug.Log($"enemyProgression: [dmg:{newEnemy.GetComponent<Attacker>().DamageAmount}, hp:{newEnemy.GetComponent<Damageable>().MaxHealth}]");
    }
}

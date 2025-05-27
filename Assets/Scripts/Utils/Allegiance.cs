using System;
using System.Collections.Generic;
using UnityEngine;

public class Allegiance
{
    public static Affinity[,] FactionStatus = {
        // Bastion,     Defenders,      Monsters
        { Affinity.Self, Affinity.Ally, Affinity.Enemy }, //Bastion
        { Affinity.Ally, Affinity.Self, Affinity.Enemy }, //Defenders
        { Affinity.Enemy, Affinity.Enemy, Affinity.Self } //Monsters
    };

    public static List<Faction> GetEnemies(Faction faction)
    {
        int factionIndex = (int)faction;
        List<Faction> enemies = new List<Faction>();

        for (int i = 0; i < FactionStatus.GetLength(1); i++)
        {
            if (FactionStatus[factionIndex, i] == Affinity.Enemy)
            {
                enemies.Add((Faction)i);
            }
        }

        return enemies;
    }
}

public enum Faction : int
{
    Bastion = 0,
    Defenders = 1,
    Monsters = 2
}

public enum Affinity : int
{
    Self = 0,
    Ally = 1,
    Enemy = 2
}
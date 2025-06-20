using System.Collections.Generic;

public class Allegiance
{
    public static Affinity[,] FactionStatus = {
        // Bastion,     Defenders,      Attachments,    Monsters
        { Affinity.Self, Affinity.Ally, Affinity.Ally, Affinity.Enemy }, //Bastion
        { Affinity.Ally, Affinity.Self, Affinity.Ally, Affinity.Enemy }, //Defenders
        { Affinity.Ally, Affinity.Ally, Affinity.Self, Affinity.Neutral }, //Attachments
        { Affinity.Enemy, Affinity.Enemy, Affinity.Neutral, Affinity.Self } //Monsters
    };

    public static List<Faction> GetEnemies(Faction faction)
    {
        int factionIndex = (int) faction;
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

    public static bool IsPlayerFaction(Faction faction)
    {
        return (faction == Faction.Bastion || faction == Faction.Defenders || faction == Faction.Attachments);
    }
}

public enum Faction : int
{
    Bastion = 0,
    Defenders = 1,
    Attachments = 2,
    Monsters = 3
}

public enum Affinity : int
{
    Self = 0,
    Ally = 1,
    Neutral = 2,
    Enemy = 3
}
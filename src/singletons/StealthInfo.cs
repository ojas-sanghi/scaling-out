using System;
using System.Collections.Generic;
using Godot;

public class StealthInfo : Node
{
    public static Enums.Genes geneBeingPursued;

    public static StealthInfo Instance;

    public Dictionary<Enums.Genes, PackedScene> geneStealthMaps;
    public Dictionary<Enums.StealthMapDifficultyLevel, PackedScene> normalStealthMaps;

    Random rng = new Random();

    public StealthInfo()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;

        geneStealthMaps = new Dictionary<Enums.Genes, PackedScene>()
        {
            { Enums.Genes.Cryo, GD.Load<PackedScene>("res://src/stealth/maps/geneMaps/StealthIce.tscn") },
            { Enums.Genes.Fire, GD.Load<PackedScene>("res://src/stealth/maps/geneMaps/StealthFire.tscn") },
        };
        normalStealthMaps = new Dictionary<Enums.StealthMapDifficultyLevel, PackedScene>()
        {

        };

    }

    // goes through list of found genes and tries to return a random unbeaten map
    // if can't find any, returns null
    public PackedScene GetUnbeatenGeneMap()
    {
        List<PackedScene> maps = new List<PackedScene>();
        foreach (KeyValuePair<Enums.Genes, PackedScene> kvp in geneStealthMaps)
        {
            if (PlayerStats.Instance.genesFound.Contains(kvp.Key))
            {
                continue;
            }
            maps.Add(kvp.Value);
        }
        
        if (maps.Count > 0)
        {
            int index = rng.Next(maps.Count);
            return maps[index];
        }
        else
        {
            return null;
        }
    }

    // returns random gene map, adherent to difficulty level
    public PackedScene GetNormalMap(Enums.StealthMapDifficultyLevel difficultyLevel)
    {
        List<PackedScene> maps = new List<PackedScene>();
        foreach (KeyValuePair<Enums.StealthMapDifficultyLevel, PackedScene> kvp in normalStealthMaps)
        {
            if (kvp.Key == difficultyLevel)
                maps.Add(kvp.Value);
        }
        int index = rng.Next(maps.Count);
        return maps[index];
    }


}
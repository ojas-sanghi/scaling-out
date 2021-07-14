using System;
using System.Linq;
using System.Collections.Generic;
using Godot;

public class StealthInfo : Node
{
    public static Enums.Genes geneBeingPursued;

    public static StealthInfo Instance;

    public Dictionary<Enums.Genes, PackedScene> geneStealthMaps;
    public Dictionary<Enums.StealthMapDifficultyLevel, PackedScene> normalStealthMaps;
    public Dictionary<Enums.StealthMapDifficultyLevel, int> difficultyGeneRewards; // how many genes are rewarded for each difficulty

    public static Random rng = new Random();
    public static double geneFindChance = 0.9; // chance to find a random gene after completing a normal stealth map

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

        difficultyGeneRewards = new Dictionary<Enums.StealthMapDifficultyLevel, int>()
        {
            { Enums.StealthMapDifficultyLevel.Easy, 100 },
            { Enums.StealthMapDifficultyLevel.Medium, 250 },
            { Enums.StealthMapDifficultyLevel.Hard, 500 },
        };  

    }

    // returns a list of genes that have not been found yet
    public List<Enums.Genes> GetNotFoundGenes()
    {
        List<Enums.Genes> allGenes = Enum.GetValues(typeof(Enums.Genes)).Cast<Enums.Genes>().ToList();

        List<Enums.Genes> notFoundGenes = allGenes.Except(PlayerStats.Instance.genesFound).ToList();
        notFoundGenes.Remove(Enums.Genes.None); // this one doesn't count >:(
        return notFoundGenes;
    }

    // gets list of unbeaten genes, takes random one, and returns corresponding map
    // if there are no unbeaten genes, returns null
    public PackedScene GetUnbeatenGeneMap()
    {
        List<Enums.Genes> notFoundGenes = GetNotFoundGenes();

        if (notFoundGenes.Count > 0)
        {
            int index = rng.Next(notFoundGenes.Count);
            Enums.Genes notFoundGene = notFoundGenes[index];
            return geneStealthMaps[notFoundGene];
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
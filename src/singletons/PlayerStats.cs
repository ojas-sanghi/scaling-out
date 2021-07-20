using Godot;
using Godot.Collections;

public class PlayerStats : Node
{
    public static int gold = 2000;
    public static int genes = 2000;

    public static PlayerStats Instance;
    public PlayerStatsResource statsResource;

    // these are pointers to the Resource file's list so it works out
    public Array<Enums.Dinos> dinosUnlocked { get; private set; }
    public Array<Enums.Genes> genesFound { get; private set; }

    public PlayerStats()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;

        // load player stats; make a new one if none exists from the pre-existing one in the project
        string statsSaveLocation = "user://PlayerStatsSave.tres";
        string projectStatsLocation = "res://src/resources/PlayerStats.tres";
        if (!ResourceLoader.Exists(statsSaveLocation))
        {
            ResourceSaver.Save(statsSaveLocation, ResourceLoader.Load<PlayerStatsResource>(projectStatsLocation));
        }
        statsResource = ResourceLoader.Load<PlayerStatsResource>("user://PlayerStatsSave.tres", null, true);

        dinosUnlocked = statsResource.dinosUnlocked;
        genesFound = statsResource.genesFound;
    }

    public void AddDinoUnlocked(Enums.Dinos dino)
    {
        dinosUnlocked.Add(dino);
        statsResource.SaveResource();
    }

    public void AddGeneFound(Enums.Genes gene)
    {
        if (!genesFound.Contains(gene))
        {
            genesFound.Add(gene);
            statsResource.SaveResource();
        }
    }
}
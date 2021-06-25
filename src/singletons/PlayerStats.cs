using Godot;
using Godot.Collections;

public class PlayerStats : Node
{
    public static int gold = 2000;
    public static int genes = 2000;

    public static PlayerStats Instance;
    public PlayerStatsResource statsResource;

    // these are pointers to the Resource file's list so it works out
    public Array<Enums.Dinos> dinosUnlocked;
    public Array<Enums.Genes> genesFound;

    public PlayerStats()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;
        statsResource = GD.Load<PlayerStatsResource>("res://src/resources/PlayerStats.tres");
        dinosUnlocked = statsResource.dinosUnlocked;
        genesFound = statsResource.genesFound;
    }
}
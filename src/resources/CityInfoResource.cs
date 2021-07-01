using Godot;
using Godot.Collections;

public class CityInfoResource : Resource
{
    [Export] public Enums.USAStates state;
    [Export] public Enums.USACities city;
    [Export] public bool capitalCity;

    [Export] public Enums.Biomes biome;

    [Export] public int levelNum;

    [Export] public Array<Enums.LaneTypes> lanesInfo;
    // [Export] public Array<Enums.ArmyGunTypes> guards; //? we doing this?

    [Export] public int rounds = 3;
    [Export] public Array<int> timePerRoundSeconds; // list of how long each round is, as many entries as there are rounds
    
    // list of how many credits player gets after each round, as many entries as there are rounds
    // note: the first entry is the amount of credits the player gets at the start of the conquest
    [Export] public Array<int> roundWinCreditBonus;
    

    [Export] public int rewardGold;

}
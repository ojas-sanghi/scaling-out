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
    
    [Export] public int rounds = 3;

    // number of NEW soliders that spawn each round; so [3, 3, 3] means 3 soliders then 6 soliders in round 2 then 9 guards in round 3
    // must be same length as rounds
    [Export] public Array<int> numSoldiersPerRound;
    // what gun each solider has
    // these correspond to each soldier in numSoldiersPerRound
    // must be same length as the sum of everything in numSoldiersPerRound
    [Export] public Array<Enums.ArmyGunTypes> soldierGunTypes;
    // what zone each soldier is in
    // these correspond to each soldier in numSoldiersPerRound
    // must be same length as the sum of everything in numSoldiersPerRound
    [Export] public Array<int> soliderZoneIndex;

    [Export] public Array<int> timePerRoundSeconds; // list of how long each round is, as many entries as there are rounds
    
    // list of how many credits player gets after each round, as many entries as there are rounds
    // note: the first entry is the amount of credits the player gets at the start of the conquest
    [Export] public Array<int> roundWinCreditBonus;

    [Export] public int rewardGold;

}
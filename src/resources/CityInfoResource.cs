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

    // number of NEW guards that spawn each round; so [3, 3, 3] means 3 guards then 6 guards in round 2 then 9 guards in round 3
    // these will be evenly spread out across the lanes, and go in order of top to bottom
    // must be same length as rounds
    [Export] public Array<int> numSoldiersPerRound;
    // what gun each guard has
    // these will be set in order from 1st round -> last round, and top to bottom
    // must be same length as the sum of everything in numSoldiersPerRound
    [Export] public Array<Enums.ArmyGunTypes> soliderGunTypes;

    [Export] public Array<int> timePerRoundSeconds; // list of how long each round is, as many entries as there are rounds
    
    // list of how many credits player gets after each round, as many entries as there are rounds
    // note: the first entry is the amount of credits the player gets at the start of the conquest
    [Export] public Array<int> roundWinCreditBonus;

    [Export] public int rewardGold;

}
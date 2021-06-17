using Godot;
using Godot.Collections;
using System.Collections.Generic;

public class CityInfoResource : Resource
{
    [Export] public Enums.USAStates state;
    [Export] public Enums.USACities city;
    [Export] public int levelNum;
    [Export] public Array<Enums.LaneTypes> lanesInfo;
    // [Export] public Array<Enums.ArmyWeapons> guards; //? we doing this?
    [Export] public int rounds = 3;
    [Export] public Array<int> timePerRoundSeconds; // list of how long each round is, as many entries as there are rounds
    // [Export] public Array<int> credsPerRound; //? how will this work??  #119

    // TODO: reward $ and genes after a conquest

}
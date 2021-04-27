using Godot;
using Godot.Collections;

public class SpecialStat : Stats
{
    [Export] public override string statName { get; set; } = "Special";
    [Export] public string special { get; set; } = "";

    // "new" so that the order is different in the exported 
    [Export] new int level { get; set; } = 0; //0 is locked, 1 is unlocked

    // hide the stats variable in the exported version
    new Array<double> stats;

    public string GetSpecial()
    {
        return special;
    }
}
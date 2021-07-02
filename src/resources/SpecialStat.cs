using Godot;
using Godot.Collections;

public class SpecialStat : Stats
{
    [Export] public override string statName { get; set; } = "Special";
    [Export] public string special { get; set; } = "";

    // hide the stats variable in the exported version
    new Array<double> stats;

    public string GetSpecial()
    {
        return special;
    }
}
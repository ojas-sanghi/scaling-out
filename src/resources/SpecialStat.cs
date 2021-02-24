using Godot;

public class SpecialStat : Stats
{
    [Export] public override string statName { get; set; } = "Special";
    [Export] public string special { get; set; } = "";

    //* A `level` of 0 for SpecialStat means not unlocked, 1 means unlocked

    public string GetSpecial()
    {
        return special;
    }
}
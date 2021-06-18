using Godot;

public class DinoInfoResource : Resource
{
    [Export] public UnlockCost unlockCost;
    [Export] public Stats hpStat;
    [Export] public Stats delayStat;
    [Export] public Stats defStat;
    [Export] public Stats dodgeStat;
    [Export] public Stats dmgStat;
    [Export] public Stats speedStat;
    [Export] public SpecialStat specialStat;
}
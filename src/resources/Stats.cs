using Godot;
using Godot.Collections;

public class Stats : Resource
{
    [Export] public virtual string statName { get; set; } = "";
    [Export] public Array<double> stats { get; set; } = new Array<double>();
    [Export] public Cost cost { get; set; }

    [Export] public int level { get; set; } = 0;

    public double GetStat()
    {
        return stats[level];
    }

    // lvl overload is for the GetNextUpgradeCost() function
    public int GetGold()
    {
        return cost.gold[level];
    }
    public int GetGold(int lvl)
    {
        return cost.gold[lvl];
    }

    public int GetGenes()
    {
        return cost.genes[level];
    }
    public int GetGenes(int lvl)
    {
        return cost.genes[lvl];
    }
}
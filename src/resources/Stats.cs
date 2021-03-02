using System.Collections.Generic;
using Godot;

public class Stats : Resource
{
    [Export] public virtual string statName { get; set; } = "";
    [Export] public List<double> stats { get; set; }
    [Export] public Cost cost { get; set; }

    [Export] public int level { get; set; } = 0;

    public virtual double GetStat()
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
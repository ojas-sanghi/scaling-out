using System.Collections.Generic;
using Godot;

public class UpgradeInfo : Node
{
    public Dictionary<Enums.Stats, Stats> stats;

    public UpgradeInfo(string path) {

        DinoInfoResource data = GD.Load<DinoInfoResource>(path);

        stats = new Dictionary<Enums.Stats, Stats>()
        {
            {Enums.Stats.Hp, data.hpStat},
            {Enums.Stats.Delay, data.delayStat},
            {Enums.Stats.Def, data.defStat},
            {Enums.Stats.Dodge, data.dodgeStat},
            {Enums.Stats.Dmg, data.dmgStat},
            {Enums.Stats.Speed, data.speedStat},
            {Enums.Stats.Special, data.specialStat},
        };
    }

    void Upgrade(Enums.Stats stat) {
        if (!IsMaxedOut(stat)) {
            stats[stat].level++;
        }
    }

    double GetStat(Enums.Stats stat) {
        if (stat == Enums.Stats.Special) {
            GD.PushError("Use GetSpecial() to get the info on a dino special!");
            GD.PrintStack();
            GetTree().Quit(1);
        }
        return stats[stat].GetStat();
    }
    string GetSpecial() {
        return ((SpecialStat) stats[Enums.Stats.Special]).GetSpecial();
    }

    int GetLevel(Enums.Stats stat) {
        return stats[stat].level;
    }

    int GetGoldCost(Enums.Stats stat) {
        return stats[stat].GetGold();
    }
    int GetGeneCost(Enums.Stats stat) {
        return stats[stat].GetGenes();
    }

    // If the user has paid and unlocked the special
    bool UnlockedSpecial() {
        return GetLevel(Enums.Stats.Special) == 1;
    }
    // If the dino has a special you can unlock
    bool HasSpecial() {
        return GetSpecial() != "";
    }

    ///////////////////////////
    // Misc utlity functions //
    ///////////////////////////

    int GetMaxLevel(Enums.Stats stat) {
        if (stat == Enums.Stats.Special) {
            if (HasSpecial()) return 1; else return 0;
        } else {
            // our level is 0-indexed but count is not, so decrement one
            return stats[stat].stats.Count - 1;
        }
    }

    // [gold, genes]
    List<int> GetNextUpgradeCost(Enums.Stats stat) {
        int CurrentLevel = GetLevel(stat);

        // no cost if already at max
        if (IsMaxedOut(stat)) {
            return new List<int>() {0, 0};
        }

        int GoldCost = stats[stat].GetGold(CurrentLevel + 1);
        int GeneCost = stats[stat].GetGenes(CurrentLevel + 1);

        return new List<int> {GoldCost, GeneCost};
    }

    bool IsMaxedOut(Enums.Stats stat) {
        return GetLevel(stat) >= GetMaxLevel(stat);
    }
    
}
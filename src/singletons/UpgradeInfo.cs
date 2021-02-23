using System.Collections.Generic;
using Godot;

public class UpgradeInfo : Node
{
    public Dictionary<Enums.Stats, Resource> stats;

    public UpgradeInfo(string path) {

        // TODO: Fix this
        // TODO: This depends on the scripts in src/resources
        // TODO: Particularly DinoInfoResource.gd, which relies on the others in that folder
        Resource data = GD.Load<Resource>(path);
        stats = new Dictionary<Enums.Stats, Resource>()
        {
            {Enums.Stats.Hp, data},
            {Enums.Stats.Delay, data},
            {Enums.Stats.Def, data},
            {Enums.Stats.Dodge, data},
            {Enums.Stats.Dmg, data},
            {Enums.Stats.Speed, data},
            {Enums.Stats.Special, data},
        };
    }

    void Upgrade(Enums.Stats stat) {
        if (!IsMaxedOut(stat)) {
            stats[stat].level += 1; // todo: fix
        }
    }

    string GetStat(Enums.Stats stat) {
        return stats[stat].get_stat(); // TODO: fix this and figure out the type
    }
    int GetLevel(Enums.Stats stat) {
        return stats[stat].level; // TODO: fix
    }

    int GetGoldCost(Enums.Stats stat) {
        return stats[stat].GetGold(); // TODO: fix
    }
    int GetGeneCost(Enums.Stats stat) {
        return stats[stat].GetGenes(); // TODO: fix
    }

    // If the suer has paid and unlocked the special
    bool UnlockedSpecial() {
        return GetLevel(Enums.Stats.Special) == 1;
    }
    // If the dino has a special you can unlock
    bool HasSpecial() {
        return true; // TODO: fix
        // GetStat(Enums.Stats.Special) != null;
    }

    /////////////////////////
    // Misc utlity functions
    /////////////////////////

    int GetMaxLevel(Enums.Stats stat) {
        if (stat == Enums.Stats.Special) {
            if (HasSpecial()) return 1; else return 0;
        } else {
            // our level is 0-indexed but size() is not, so decrement one
            return stats[stat].stats.size() - 1; // TODO: fix
        }
    }

    // [gold, genes]
    List<int> GetNextUpgradeCost(Enums.Stats stat) {
        int CurrentLevel = GetLevel(stat);

        // no cost if already at max
        if (IsMaxedOut(stat)) {
            return new List<int>() {0, 0};
        }

        // TODO: fix these
        int GoldCost = stats[stat].GetGold(CurrentLevel + 1);
        int GeneCost = stats[stat].GetGenes(CurrentLevel + 1);

        return new List<int> {GoldCost, GeneCost};
    }

    bool IsMaxedOut(Enums.Stats stat) {
        return GetLevel(stat) >= GetMaxLevel(stat);
    }
    
}
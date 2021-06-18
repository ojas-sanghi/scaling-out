using Godot;
using e = Enums;

public class Test : Node
{
    public override void _Ready()
    {
        GD.Print("start");
        // UpgradeInfo u = new UpgradeInfo("res://src/actors/dinos/stats/MegaDino.tres");

        // var statsdict = u.stats;
        // var hp_stats = statsdict[Enums.Stats.Hp];

        // GD.Print(hp_stats);
        // GD.Print(hp_stats.GetType());


        // GD.Print("before statname ands tata");

        // GD.Print(hp_stats.statName);
        // GD.Print(hp_stats.stats);

        // GD.Print("after statname ands tats");

        // var hp = u.GetStat(Enums.Stats.Hp);
        // GD.Print(hp);

        // UpgradeInfo u = new UpgradeInfo("res://src/actors/dinos/stats/MegaDino.tres");
        // GD.Print(u);
        // GD.Print("end");

        // GD.Print(e.EnumToString.GetUSAStateName(e.USAStates.Alabama));
        // GD.Print(e.EnumToString.GetUSAStateName(e.USAStates.RhodeIsland));

        string pathStart = "res://src/combat/levelsInfo/Level";
        string pathEnd = "Info.tres";

        GD.Print(pathStart + 1 + pathEnd);
        GD.Print((pathStart + 1 + pathEnd).GetType());
        GD.Print(pathStart + 2 + pathEnd);
        GD.Print((pathStart + 2 + pathEnd).GetType());



    }


}
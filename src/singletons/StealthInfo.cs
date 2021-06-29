using System.Collections.Generic;
using Godot;

public class StealthInfo : Node
{
    public static Enums.Genes geneBeingPursued;

    public static StealthInfo Instance;

    public Dictionary<Enums.Genes, PackedScene> geneStealthMaps;

    public StealthInfo()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Instance = this;

        geneStealthMaps = new Dictionary<Enums.Genes, PackedScene>()
        {
            { Enums.Genes.Cryo, GD.Load<PackedScene>("res://src/stealth/StealthIce.tscn") },
            { Enums.Genes.Fire, GD.Load<PackedScene>("res://src/stealth/StealthFire.tscn") },
        };

    }

}
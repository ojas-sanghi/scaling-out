using Godot;
using System.Collections.Generic

public class StealthInfo : Node
{
    public static Enums.Genes geneBeingPursued;

    StealthInfo Instance;

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
            { Enums.Genes.Ice, GD.Load<PackedScene>("res://src/stealth/StealthIce.tscn") },
            { Enums.Genes.Fire, GD.Load<PackedScene>("res://src/stealth/StealthFire.tscn") },
        };

    }

}
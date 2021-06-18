using System.Collections.Generic;
using Enums;
using Godot;

public class ShopInfo : Node
{
    public static List<Genes> specialGenes = new List<Genes>() { Genes.Ice, Genes.Fire };

    public static Dinos shopDino = Enums.Dinos.Tanky;
}
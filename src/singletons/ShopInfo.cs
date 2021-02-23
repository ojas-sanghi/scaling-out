using System.Collections.Generic;
using Enums;
using Godot;

public class ShopInfo : Node
{

    public static bool playerCaught = false;

    public static int gold = 30000;
    public static int genes = 30000;
    public static List<Genes> specialGenes = new List<Genes>() { Genes.Ice, Genes.Fire };

    public static Dinos shopDino = Enums.Dinos.Tanky;

}
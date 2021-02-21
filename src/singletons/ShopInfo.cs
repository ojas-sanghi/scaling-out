using Godot;
using System.Collections.Generic;

using Enums;
using g = Enums.SpecialGenes;

public class ShopInfo : Node
{

    public static bool playerCaught = false;

    public static int gold = 30000;
    public static int genes = 30000;
    public static List<g> specialGenes = new List<g>() { g.Ice, g.Fire };

    public static Dinos shopDino = Enums.Dinos.Tanky;

}
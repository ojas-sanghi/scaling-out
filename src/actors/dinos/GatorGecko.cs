using System.Collections.Generic;

public class GatorGecko : BaseDino
{

    public GatorGecko()
    {
        CalculateUpgrades();

        dinoType = Enums.Dinos.Gator;
        specialGene = Enums.Genes.Florida;

        dinoUnlockCost = new List<int>() { 50, 50 };
    }

    public override void _Ready()
    {
        base._Ready();
    }

}
using System.Collections.Generic;

public class MegaDino : BaseDino
{
    public MegaDino()
    {
        dinoType = Enums.Dinos.Mega;
        CalculateUpgrades();

        specialGene = Enums.Genes.None;

        dinoUnlockCost = new List<int>() { 10, 10 };
    }

    public override void _Ready()
    {
        base._Ready();
    }

}
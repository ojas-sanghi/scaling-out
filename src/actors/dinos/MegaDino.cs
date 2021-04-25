using System.Collections.Generic;
using Godot;

public class MegaDino : BaseDino
{
    public MegaDino()
    {
        CalculateUpgrades();

        dinoType = Enums.Dinos.Mega;
        specialGene = Enums.Genes.None;

        dinoUnlockCost = new List<int>() { 10, 10 };
    }

    public override void _Ready()
    {
        base._Ready();
    }

}
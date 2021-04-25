using Godot;
using System.Collections.Generic;

public class MegaDino : BaseDino
{

    public override void _Ready()
    {
        base._Ready();
        
        dinoType = Enums.Dinos.Mega;
        specialGene = Enums.Genes.None;

        dinoUnlockCost = new List<int>() { 10, 10 };

        CalculateUpgrades();
        
    }

}
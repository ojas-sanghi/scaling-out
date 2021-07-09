using System.Collections.Generic;

public class WarriorDino : AbilityDino
{
    DinoProjectile fireProjectile;

    public WarriorDino()
    {
        dinoType = Enums.Dinos.Warrior;
        CalculateUpgrades();

        specialGene = Enums.Genes.Fire;

        dinoUnlockCost = new List<int>() { 50, 60 };
    }
}
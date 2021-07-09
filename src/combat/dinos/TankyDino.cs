using System.Collections.Generic;

public class TankyDino : AbilityDino
{
    DinoProjectile iceProjectile;

    public TankyDino()
    {
        dinoType = Enums.Dinos.Tanky;
        CalculateUpgrades();

        specialGene = Enums.Genes.Cryo;

        dinoUnlockCost = new List<int>() { 25, 50 };
    }
}
using System.Collections.Generic;

public class GaterGecko : BaseDino {

    public GaterGecko() {
        dinoType = Enums.Dinos.Gator;
        specialGene = Enums.Genes.Florida;

        dinoUnlockCost = new List<int>() {50, 50};

        CalculateUpgrades();
    }

}
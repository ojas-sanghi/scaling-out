using System.Collections.Generic;

public class TankyDino : BaseDino {

    DinoProjectile iceProjectile;

    public TankyDino() {
        dinoType = Enums.Dinos.Tanky;
        specialGene = Enums.Genes.Ice;

        dinoUnlockCost = new List<int>() {25, 50};

        CalculateUpgrades();
    }

    public override void _Ready() {
        iceProjectile.Hide();
    }

    void ShootProjectile() {
        iceProjectile.Show();
        iceProjectile.disabled = true;
    }

}
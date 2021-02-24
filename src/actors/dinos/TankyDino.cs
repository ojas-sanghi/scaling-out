using Godot;
using System.Collections.Generic;

public class TankyDino : BaseDino {

    Area2D iceProjectile;

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
        // todo: code iceProjectile and disable it
    }

}
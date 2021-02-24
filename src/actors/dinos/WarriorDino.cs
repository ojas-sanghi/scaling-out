using Godot;
using System.Collections.Generic;

public class WarriorDino : BaseDino {

    Area2D fireProjectile;

    public WarriorDino() {
        dinoType = Enums.Dinos.Warrior;
        specialGene = Enums.Genes.Fire;

        dinoUnlockCost = new List<int>() {50, 60};

        CalculateUpgrades();
    }

    public override void _Ready() {
        fireProjectile.Hide();
    }

    void ShootProjectile() {
        fireProjectile.Show();
        // todo: code fireProjectile and disable it
    }

}
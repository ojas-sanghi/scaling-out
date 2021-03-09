using System.Collections.Generic;

public class WarriorDino : BaseDino
{

    DinoProjectile fireProjectile;

    public override void _Ready()
    {
        base._Ready();

        dinoType = Enums.Dinos.Warrior;
        specialGene = Enums.Genes.Fire;

        dinoUnlockCost = new List<int>() { 50, 60 };

        CalculateUpgrades();
        fireProjectile = (DinoProjectile)FindNode("FireProjectile");
        fireProjectile.Hide();
    }

    public void ShootProjectile()
    {
        fireProjectile.Show();
        fireProjectile.disabled = true;
    }

}
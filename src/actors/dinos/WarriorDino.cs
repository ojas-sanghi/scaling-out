using System.Collections.Generic;

public class WarriorDino : BaseDino
{
    DinoProjectile fireProjectile;

    public WarriorDino()
    {
        CalculateUpgrades();

        dinoType = Enums.Dinos.Warrior;
        specialGene = Enums.Genes.Fire;

        dinoUnlockCost = new List<int>() { 50, 60 };
    }

    public override void _Ready()
    {
        base._Ready();

        fireProjectile = (DinoProjectile)FindNode("FireProjectile");
        fireProjectile.Hide();
    }

    public void ShootProjectile()
    {
        fireProjectile.Show();
        fireProjectile.disabled = false;
    }

}
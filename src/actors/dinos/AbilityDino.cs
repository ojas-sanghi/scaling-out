using Godot;

public class AbilityDino : BaseDino
{
    DinoProjectile abilityProjectile;

    public override void _Ready()
    {
        abilityProjectile = GetNode<DinoProjectile>("AbilityProjectile");
        abilityProjectile.Hide();

        base._Ready();
    }

    public void ShootProjectile()
    {
        abilityProjectile.Show();
        abilityProjectile.disabled = false;
    }

}
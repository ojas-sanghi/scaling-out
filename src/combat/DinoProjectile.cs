using Godot;
using System.Linq;

public class DinoProjectile : Area2D
{
    [Export] Enums.SpecialAbilities type = Enums.SpecialAbilities.None;

    Vector2 speed = new Vector2(600, 0);
    
    protected bool isEnhanced;

    public bool disabled = true;
    

    public override void _Ready()
    {
        if (type == Enums.SpecialAbilities.None)
        {
            GD.PushError("DinoProjectile type must be set");
            GD.PrintStack();
            GetTree().Quit(1);
        }
        this.Hide();

        Enums.Dinos associatedDino = DinoInfo.Instance.GetDinoTypeFromAbility(this.type);
        Enums.Biomes currentBiome = CityInfo.Instance.currentCity.biome;
        isEnhanced = CityInfo.Instance.biomeFavoredDinos[currentBiome].Contains(associatedDino);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!disabled)
        {
            this.Show();
            Position += speed * delta;
        }
    }

    public virtual void OnDinoProjectileAreaEntered(Area2D area)
    {
        if (disabled) return;

        // TODO: figure out what happens when multiple ice projectiles hit the barrrier
        // maybe ignore them once the first one hits?

        Events.publishProjectileHit(type);
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
    }
}
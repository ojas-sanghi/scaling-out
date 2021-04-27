using Godot;

public class DinoProjectile : Area2D
{

    [Export] Enums.Genes type = Enums.Genes.None;

    Vector2 speed = new Vector2(600, 0);

    public bool disabled = true;


    public override void _Ready()
    {
        if (type == Enums.Genes.None)
        {
            GD.PushError("DinoProjectile type must be set");
            GD.PrintStack();
            GetTree().Quit(1);
        }
        this.Hide();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!disabled)
        {
            this.Show();
            Position += speed * delta;
        }
    }

    void _on_DinoProjectile_area_entered(Area2D area)
    {
        if (!disabled)
        {
            Events.publishProjectileHit(type);
        }
    }

}
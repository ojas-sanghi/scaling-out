using Godot;

public class BaseGuard : Area2D
{
    AnimatedSprite animSprite;
    CanvasLayer canvasLayer;

    public override void _Ready()
    {
        animSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        canvasLayer = GetNode<CanvasLayer>("CanvasLayer");

        GD.Randomize();
        uint random = GD.Randi() % 3;

        Enums.ArmyGunTypes weapon = (Enums.ArmyGunTypes)random;
        string animString = "move_" + weapon.ToString().ToLower();
        animSprite.Play(animString);

        // for ALL the guards, not just the one who found the scientist
        Events.levelFailed += OnLevelFailed;
    }

    public override void _Process(float delta)
    {
        Transform2D newtr = new Transform2D(new Vector2(1, 0), new Vector2(0, 1), this.GlobalPosition + ((Node2D)GetParent().GetParent()).GlobalPosition);
        // GD.Print(newtr);
        canvasLayer.Transform = newtr;
    }

    void OnBaseGuardBodyEntered(Node2D body)
    {
        Events.publishLevelFailed();
    }

    void OnLevelFailed()
    {
        animSprite.Stop();
    }
}
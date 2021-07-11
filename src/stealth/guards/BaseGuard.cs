using Godot;

public class BaseGuard : Area2D
{
    AnimatedSprite animSprite;

    public override void _Ready()
    {
        SetProcess(false);

        animSprite = (AnimatedSprite)FindNode("AnimatedSprite");

        GD.Randomize();
        uint random = GD.Randi() % 3;

        Enums.ArmyGunTypes weapon = (Enums.ArmyGunTypes)random;
        string animString = "move_" + weapon.ToString().ToLower();
        animSprite.Play(animString);

        // for ALL the guards, not just the one who found the scientist
        Events.levelFailed += OnLevelFailed;
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
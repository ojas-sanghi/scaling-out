using Godot;

public class BaseGuard : Area2D
{
    AnimatedSprite animSprite;
    Light2D light2D;

    Color caughtColor = new Color(1, 0, 0, 1);
    Color alertColor = new Color(1, 1, 0, 1);

    bool alerted = false;

    public override void _Ready()
    {
        animSprite = (AnimatedSprite)FindNode("AnimatedSprite");
        light2D = GetNode<Light2D>("GuardFOV/Light2D");

        GD.Randomize();
        uint random = GD.Randi() % 3;

        Enums.ArmyGunTypes weapon = (Enums.ArmyGunTypes)random;
        string animString = "move_" + weapon.ToString().ToLower();
        animSprite.Play(animString);

        Events.scientistEnteredWarnZone += OnScientistEnteredWarnZone;

        // for ALL the guards, not just the one who found the scientist
        Events.levelFailed += OnLevelFailed;
    }

    void OnScientistEnteredWarnZone()
    {
        alerted = true;;
        light2D.Color = alertColor;
    }

    void OnBaseGuardBodyEntered(Node2D body)
    {
        light2D.Color = caughtColor;
        Events.publishLevelFailed();
    }

    void OnLevelFailed()
    {
        animSprite.Stop();
    }
}
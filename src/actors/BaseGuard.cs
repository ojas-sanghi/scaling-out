using Godot;

public class BaseGuard : Area2D
{

    public override void _Ready()
    {
        var animSprite = (AnimatedSprite)FindNode("AnimatedSprite");

        GD.Randomize();
        uint random = GD.Randi() % 3;
        // TODO: convert this to an enum
        string weapon = "rifle";
        switch (random)
        {
            case 0:
                weapon = "rifle";
                break;
            case 1:
                weapon = "handgun";
                break;
            case 2:
                weapon = "shotgun";
                break;
        }
        string animString = "move_" + weapon;
        animSprite.Play(animString);
    }

    //? Does this signal function work?
    //? Is this connected to the FOV code? Do we change this?
    void _on_BaseGuard_body_entered(Node2D body)
    {
        // var rotateDeg = Mathf.
        var rotateBy = Position.AngleTo(body.Position);
        Rotation += rotateBy;

        Events.publishLevelFailed();
    }


}
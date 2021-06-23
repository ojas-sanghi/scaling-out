using Godot;
using System;

public class BaseGuard : Area2D
{

    public override void _Ready()
    {
        var animSprite = (AnimatedSprite)FindNode("AnimatedSprite");

        GD.Randomize();
        uint random = GD.Randi() % 3;

        Enums.ArmyGunTypes weapon = (Enums.ArmyGunTypes)random;

        string animString = "move_" + weapon.ToString();
        animSprite.Play(animString);
    }

    //? Is this connected to the FOV code? Do we change this?
    void OnBaseGuardBodyEntered(Node2D body)
    {
        GD.Print("body entered");
        // var rotateDeg = Mathf.
        var rotateBy = Position.AngleTo(body.Position);
        Rotation += rotateBy;

        Events.publishLevelFailed();
    }


}
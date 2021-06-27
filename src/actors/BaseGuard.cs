using Godot;
using Godot.Collections;
using System;

public class BaseGuard : Area2D
{
    AnimatedSprite animSprite;

    Area2D FOVArea;
    CollisionPolygon2D FOVCollision;

    public override void _Ready()
    {
        animSprite = (AnimatedSprite)FindNode("AnimatedSprite");

        FOVArea = GetNode<Area2D>("FieldOfView");
        FOVCollision = (CollisionPolygon2D)FindNode("FOVCollision");

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
        return;
        if (body is KinematicBody2D)
        {
            Events.publishLevelFailed();
            return;
        }
        // else, it's a tilemap; update the FOV cone
        TileMap tileMap = (TileMap)body;

        var tileCollisionPos = tileMap.MapToWorld(tileMap.WorldToMap(new Vector2(325, 0)));
        GD.Print(tileMap.WorldToMap(FOVCollision.GlobalPosition));
        var diff = FOVCollision.GlobalPosition - tileCollisionPos;
        GD.Print(FOVCollision.GlobalPosition);
        GD.Print(tileCollisionPos);
        GD.Print(diff);

        if (diff != null)
        {
            var newX = diff.x;
            var newY = FOVCollision.Polygon[1].y;
            var newVectorList = new Vector2[] { Vector2.Zero, new Vector2(newX, newY), new Vector2(newX, -newY) };
            //  Polygon = newVectorList;
            FOVCollision.SetDeferred("polygon", newVectorList);
            Update();
        }
    }

    void OnLevelFailed()
    {
        animSprite.Stop();
    }

    public override void _Draw()
    {
        DrawColoredPolygon(FOVCollision.Polygon, Colors.Red);
    }
}
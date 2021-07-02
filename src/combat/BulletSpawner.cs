using System.Collections.Generic;
using Enums;
using Godot;

public class BulletSpawner : Node2D
{
    PackedScene bullet = GD.Load<PackedScene>("res://src/combat/Bullet.tscn");
    PackedScene bulletGroupScene = GD.Load<PackedScene>("res://src/combat/BulletsGroup.tscn");

    List<Bullet> bullets = new List<Bullet>();

    public ArmyGunTypes mode;
    public int bulletSpeed = 200;

    void NewBullet()
    {
        Bullet newBullet = (Bullet)bullet.Instance();
        newBullet.speed = bulletSpeed;
        newBullet.mode = mode;
        bullets.Add(newBullet);
    }

    void SpawnBullets()
    {
        bullets.Clear();

        BulletsGroup bulletsGroup = (BulletsGroup)bulletGroupScene.Instance();
        bulletsGroup.speed = bulletSpeed;
        bulletsGroup.RotationDegrees = (float)GD.RandRange(-5, 5);

        if (mode == ArmyGunTypes.Shotgun)
        {
            // make three new bullets
            foreach (int i in GD.Range(0, 3))
            {
                NewBullet();
            }
            bullets[0].RotationDegrees = 5;
            bullets[2].RotationDegrees = -5;
        }
        else
        {
            // one new bullet
            NewBullet();
        }

        foreach (Bullet b in bullets)
        {
            bulletsGroup.AddChild(b);
        }
        // add bullets to combatscreen to ensure they wont randomly change direction when the army person rotates
        bulletsGroup.RotationDegrees = ((ArmyDude)GetParent()).RotationDegrees;
        bulletsGroup.GlobalPosition = GlobalPosition;
        GetNode("/root/" + CombatInfo.combatScreenName).AddChild(bulletsGroup);

        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();

    }

}
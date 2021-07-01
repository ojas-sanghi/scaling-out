using System.Collections.Generic;
using Enums;
using Godot;

public class BulletSpawner : Node2D
{
    PackedScene bullet = GD.Load<PackedScene>("res://src/combat/bullets/Bullet.tscn");
    PackedScene bulletGroupScene = GD.Load<PackedScene>("res://src/combat/bullets/BulletsGroup.tscn");

    List<Bullet> bullets = new List<Bullet>();

    public ArmyGunTypes gunType;
    public int bulletSpeed = 200;

    void NewBullet()
    {
        Bullet newBullet = (Bullet)bullet.Instance();
        newBullet.speed = bulletSpeed;
        newBullet.gunType = gunType;
        bullets.Add(newBullet);
    }

    void SpawnBullets()
    {
        bullets.Clear();

        BulletsGroup bulletsGroup = (BulletsGroup)bulletGroupScene.Instance();
        bulletsGroup.speed = bulletSpeed;

        if (gunType == ArmyGunTypes.Shotgun)
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
        bulletsGroup.RotationDegrees = ((CombatArmySoldier)GetParent()).RotationDegrees;
        bulletsGroup.RotationDegrees += (float)GD.RandRange(-5, 5);
        bulletsGroup.GlobalPosition = GlobalPosition;
        GetNode("/root/CombatScreen").AddChild(bulletsGroup);

        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();

    }

}
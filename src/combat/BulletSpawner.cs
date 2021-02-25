using System.Collections.Generic;
using Godot;

public class BulletSpawner : Node2D
{
    PackedScene bullet = GD.Load<PackedScene>("res://src/combat/Bullet.tscn");
    PackedScene bulletGroupScene = GD.Load<PackedScene>("res://src/combat/BulletGroup.tscn");

    List<Bullet> bullets = new List<Bullet>();

    public string mode;

    void NewBullet()
    {
        Bullet newBullet = (Bullet)bullet.Instance();
        newBullet.mode = mode;
        bullets.Add(newBullet);
    }

    void SpawnBullets()
    {
        bullets.Clear();

        BulletsGroup bulletsGroup = (BulletsGroup)bulletGroupScene.Instance();
        bulletsGroup.RotationDegrees = (float)GD.RandRange(-5, 5);

        if (mode == "shotgun")
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
        AddChild(bulletsGroup);
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();

    }

}
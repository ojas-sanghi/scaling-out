using Enums;
using Godot;

public class Bullet : Area2D
{
    public ArmyGunTypes mode = ArmyGunTypes.Pistol;

    public int speed;
    int bulletDmg = 14;

    public override void _Ready()
    {
        Timer timer = (Timer)FindNode("ExistenceTimer");

        if (mode == ArmyGunTypes.Shotgun)
        {
            bulletDmg = 6;
            timer.WaitTime = 0.75f;
        }
        else
        {
            bulletDmg = 4;
        }
        timer.Start();
    }


    public override void _PhysicsProcess(float delta)
    {
        Vector2 bulletVel = Vector2.Right.Rotated(Rotation) * speed;
        Position += bulletVel * delta;
    }

    void OnBulletAreaEntered(BaseDino dino)
    {
        dino.UpdateHealth(bulletDmg);
        QueueFree();
    }

    // Damage dropoff after certain time
    void OnExistenceTimerTimeout()
    {
        bulletDmg = 1;
    }

    void OnViewportExited(Viewport viewport)
    {
        QueueFree();
    }

}
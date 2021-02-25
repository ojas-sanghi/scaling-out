using Godot;

public class Bullet : Area2D
{
    // TODO: covert this to an enum
    public string mode = "pistol";

    int speed = CombatInfo.Instance.bulletSpeed / 2;
    int bulletDmg = 14;

    public override void _Ready()
    {
        Timer timer = (Timer) FindNode("ExistenceTimer");

        if (mode == "shotgun") {
            bulletDmg = 6;
            timer.WaitTime = (float)0.75;
        } else {
            bulletDmg = 4;
        }
        timer.Start();
    }


    public override void _PhysicsProcess(float delta)
    {
        Vector2 bulletVel = Vector2.Left.Rotated(Rotation) * speed;
        Position += bulletVel * delta;
    }

    //? Does this connection work
    void _on_Bullet_area_entered(BaseDino dino) {
        dino.UpdateHealth(bulletDmg);
        QueueFree();
    }

    //? Does this connection work
    // Damage dropoff after certain time
    void _on_ExistenceTimer_timeout() {
        bulletDmg = 1;
    }

}
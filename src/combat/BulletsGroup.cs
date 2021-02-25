using Godot;

public class BulletsGroup : Area2D
{
    int speed = CombatInfo.Instance.bulletSpeed / 2;

    public override void _PhysicsProcess(float delta)
    {
        Vector2 velocity = Vector2.Left.Rotated(Rotation) * speed;
        Position += velocity * delta;
    }

    void _on_VisibilityNotifier2D_screen_exited()
    {
        QueueFree();
    }


}
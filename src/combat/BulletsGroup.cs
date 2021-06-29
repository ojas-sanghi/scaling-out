using Godot;

public class BulletsGroup : Area2D
{
    public int speed;

    public override void _PhysicsProcess(float delta)
    {
        Vector2 velocity = Vector2.Right.Rotated(Rotation) * speed;
        Position += velocity * delta;
    }

    void _on_VisibilityNotifier2D_screen_exited()
    {
        QueueFree();
    }


}
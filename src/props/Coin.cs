using Godot;

public class Coin : Area2D
{
    [Export] int value = 1;

    Tween effectTween;

    public override void _Ready()
    {
        effectTween = (Tween)FindNode("Effect");
        var sprite = (Sprite)FindNode("Sprite");

        // Increase size
        effectTween.InterpolateProperty(
            sprite, "scale", sprite.Scale, new Vector2(2, 2), (float)0.6, Tween.TransitionType.Cubic, Tween.EaseType.Out
        ); ;
        // Fade out
        effectTween.InterpolateProperty(
            sprite, "modulate", new Color(1, 1, 1, 1), new Color(0, 0, 0, 0), (float)0.6, Tween.TransitionType.Linear, Tween.EaseType.Out
        );
    }

    async void OnCoinBodyEntered(Node body)
    {
        var collision = (CollisionShape2D)FindNode("CollisionShape2D");
        // Turn off collision to make sure no extra collisions while the effect is playinf
        collision.SetDeferred("disabled", true);
        // Start and wait for tween to finish
        effectTween.Start();
        await ToSignal(effectTween, "tween_completed");
        // Emit signal and remove coin
        Events.publishCoinGrabbed(value);
        QueueFree();
    }

}
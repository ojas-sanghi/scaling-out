using System.Threading.Tasks;
using Godot;

public class PathFollowGuard : Path2D
{
    PathFollow2D follow;
    BaseGuard baseGuard;
    Tween tween;

    [Export] int walkTime = 4;
    [Export] float turnTime = 1.5f;
    [Export] float pauseTime = 0.5f;

    public override void _Ready()
    {
        follow = (PathFollow2D)FindNode("Follow");
        baseGuard = (BaseGuard)FindNode("BaseGuard");
        tween = (Tween)baseGuard.GetNode("Tween");

        Events.levelFailed += OnLevelFailed;

        ForwardTween();
    }

    async void ForwardTween()
    {
        tween.InterpolateProperty(
            follow, "unit_offset", 0, 1, walkTime, Tween.TransitionType.Linear, Tween.EaseType.InOut
        );
        tween.Start();

        await ToSignal(tween, "tween_completed");

        await Turn180Tween();
        BackwardTween();
    }

    async void BackwardTween()
    {
        // pause before turning
        await ToSignal(GetTree().CreateTimer(pauseTime, false), "timeout");

        tween.InterpolateProperty(
            follow, "unit_offset", 1, 0, walkTime, Tween.TransitionType.Linear, Tween.EaseType.InOut
        );
        // baseGuard.RotationDegrees = 180;
        tween.Start();

        await ToSignal(tween, "tween_completed");

        await Turn180Tween();
        ForwardTween();
    }

    async Task Turn180Tween()
    {
        // if we're at 0 degrees, turn to 180
        // else, go back to 0 degrees
        var degToTurnTo = baseGuard.RotationDegrees == 0 ? 180 : 0;

        tween.InterpolateProperty(
            baseGuard, "rotation_degrees", null, degToTurnTo, turnTime, Tween.TransitionType.Sine, Tween.EaseType.InOut
        );
        tween.Start();

        await ToSignal(tween, "tween_completed");
    }

    void OnLevelFailed()
    {
        tween.StopAll();
    }

}
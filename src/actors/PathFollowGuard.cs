using Godot;
using System.Threading.Tasks;

public class PathFollowGuard : Path2D
{
    PathFollow2D follow;
    BaseGuard baseGuard;
    Tween tween;

    int walkTime = 4;
    float turnTime = 1.5f;
    float pauseTime = 0.5f;

    public override void _Ready()
    {
        follow = (PathFollow2D)FindNode("Follow");
        baseGuard = (BaseGuard)FindNode("BaseGuard");
        tween = (Tween)baseGuard.GetNode("Tween");

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
            baseGuard, "rotation_degrees", null, degToTurnTo, turnTime
        );
        tween.Start();

        await ToSignal(tween, "tween_completed");
    }

}
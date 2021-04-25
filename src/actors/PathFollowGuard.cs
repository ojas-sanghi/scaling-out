using Godot;

public class PathFollowGuard : Path2D
{
    PathFollow2D follow;
    BaseGuard baseGuard;
    Tween forwardTween;
    Tween backwardTween;

    int walkTime = 4;

    public override void _Ready()
    {
        follow = (PathFollow2D)FindNode("Follow");
        baseGuard = (BaseGuard)FindNode("BaseGuard");
        forwardTween = (Tween)baseGuard.GetNode("Forward");
        backwardTween = (Tween)baseGuard.GetNode("Backward");

        ForwardTween();
    }

    async void ForwardTween()
    {
        forwardTween.InterpolateProperty(
            follow, "unit_offset", 0, 1, walkTime, Tween.TransitionType.Linear, Tween.EaseType.InOut
        );
        baseGuard.RotationDegrees = 0;
        forwardTween.Start();

        await ToSignal(forwardTween, "tween_completed");
        BackwardTween();
    }

    async void BackwardTween()
    {
        backwardTween.InterpolateProperty(
            follow, "unit_offset", 1, 0, walkTime, Tween.TransitionType.Linear, Tween.EaseType.InOut
        );
        baseGuard.RotationDegrees = 180;
        backwardTween.Start();

        await ToSignal(backwardTween, "tween_completed");
        ForwardTween();
    }



}
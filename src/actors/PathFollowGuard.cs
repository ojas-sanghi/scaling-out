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

        // Start the other tween once one finishes
        forwardTween.Connect("tween_completed", this, "BackwardTween");
        backwardTween.Connect("tween_completed", this, "ForwardTween");
    }

    void ForwardTween()
    {
        //? does this property work
        forwardTween.InterpolateProperty(
            follow, "UnitOffset", 0, 1, walkTime, Tween.TransitionType.Linear, Tween.EaseType.InOut
        );
        baseGuard.RotationDegrees = 0;
        forwardTween.Start();
    }

    void BackwardTween()
    {
        //? does this property work
        backwardTween.InterpolateProperty(
            follow, "UnitOffset", 1, 0, walkTime, Tween.TransitionType.Linear, Tween.EaseType.InOut
        );
        baseGuard.RotationDegrees = 180;
        backwardTween.Start();
    }



}
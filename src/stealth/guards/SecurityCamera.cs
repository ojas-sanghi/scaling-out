using Godot;
using System;

public class SecurityCamera : Node2D
{
    [Export] float rotationDegEitherSide = 60;
    [Export] float rotationTimeSec = 5;
    [Export] float postRotationDelay = 2; // how long camera pauses after finishing a rotation

    GuardFOV FOVNode;
    Tween tween;

    public override void _Ready()
    {
        Events.scientistEnteredWarnZone += OnScientistEnteredWarnZone;
        tween = GetNode<Tween>("Tween");
        FOVNode = GetNode<GuardFOV>("FOV");

        RotateLeft();
    }

    async void RotateLeft()
    {
        tween.InterpolateProperty(this, "rotation_degrees", null, rotationDegEitherSide, rotationTimeSec, Tween.TransitionType.Sine, Tween.EaseType.InOut);
        tween.Start();
        await ToSignal(tween, "tween_completed");
        await ToSignal(GetTree().CreateTimer(postRotationDelay, false), "timeout");
        RotateRight();
    }

    async void RotateRight()
    {
        tween.InterpolateProperty(this, "rotation_degrees", null, -rotationDegEitherSide, rotationTimeSec, Tween.TransitionType.Sine, Tween.EaseType.InOut);
        tween.Start();
        await ToSignal(tween, "tween_completed");
        await ToSignal(GetTree().CreateTimer(postRotationDelay, false), "timeout");
        RotateLeft();
    }

    void OnScientistEnteredWarnZone()
    {

        if (FOVNode.inWarnArea.Count > 0)
        {
            Vector2 scientistPos = ((Node2D)(FOVNode.inWarnArea[0])).GlobalPosition;
            Events.publishScientistEnteredCameraZone(scientistPos);

            // TODO: make the FOV yellow and add an exclamaion mark
        }

        // along with this: figure out if we're doing warn-system with guards, and how that wll work
    }
}

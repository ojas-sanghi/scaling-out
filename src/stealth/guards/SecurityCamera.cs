using Godot;
using System;

public class SecurityCamera : Node2D
{
    [Export] float rotationDegEitherSide = 80;
    [Export] float rotationTimeSec = 5;
    [Export] float postRotationDelay = 2; // how long camera pauses after finishing a rotation

    GuardFOV FOVNode;
    Tween tween;

    public override void _Ready()
    {
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
}

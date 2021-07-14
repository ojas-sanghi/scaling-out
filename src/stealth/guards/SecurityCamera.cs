using Godot;
using System;

public class SecurityCamera : Node2D
{
    [Export] float rotationDegEitherSide = 80;
    [Export] float rotationTimeSec = 5;
    [Export] float postRotationDelay = 2; // how long camera pauses after finishing a rotation

    GuardFOV FOVNode;
    Tween tween;

    bool lostLevel = false; // don't move if we've lost the level. just signals and tween.StopAll() dont work :/

    public override void _Ready()
    {
        tween = GetNode<Tween>("Tween");
        FOVNode = GetNode<GuardFOV>("FOV");

        Events.levelFailed += OnLevelFailed; // it's a one-shot signal
        Events.newRound += OnNewRound;

        RotateLeft();
    }

    public override void _ExitTree()
    {
        Events.levelFailed -= OnLevelFailed;
        Events.newRound -= OnNewRound;
    }

    void OnNewRound()
    {
        Events.levelFailed += OnLevelFailed; // it's a one-shot signal
    }

    async void RotateLeft()
    {
        if (lostLevel) return;

        tween.InterpolateProperty(this, "rotation_degrees", null, rotationDegEitherSide, rotationTimeSec, Tween.TransitionType.Sine, Tween.EaseType.InOut);
        tween.Start();
        await ToSignal(tween, "tween_completed");
        await ToSignal(GetTree().CreateTimer(postRotationDelay, false), "timeout");
        RotateRight();
    }

    async void RotateRight()
    {
        if (lostLevel) return;

        tween.InterpolateProperty(this, "rotation_degrees", null, -rotationDegEitherSide, rotationTimeSec, Tween.TransitionType.Sine, Tween.EaseType.InOut);
        tween.Start();
        await ToSignal(tween, "tween_completed");
        await ToSignal(GetTree().CreateTimer(postRotationDelay, false), "timeout");
        RotateLeft();
    }

    void OnLevelFailed()
    {
        Events.levelFailed -= OnLevelFailed; // it's a one-shot signal
        tween.StopAll();
        lostLevel = true;
    }
}

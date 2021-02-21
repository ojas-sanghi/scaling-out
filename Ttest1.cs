using Godot;
using System;

public class Ttest1 : Node2D
{
    public override void _Ready()
    {
        Events.eventNormal += onNormal;
        Events.eventWithNumber += onNumber;
    }

    public override void _ExitTree()
    {
        Events.eventNormal -= onNormal;
        Events.eventWithNumber -= onNumber;
    }

    void onNormal() {
        GD.Print("normal thing");
    }

    void onNumber(int number) {
        GD.Print("number thing! " + number.ToString());
    }

    void _on_Button_pressed() {
        Events.publishNormal();
    }

    void _on_Button2_pressed() {
        Events.publishNumber();
    }

}

using Godot.Collections;
using Godot;

public class Cost : Resource
{
    [Export] string statName = ""; // Not used but there so we can figure out what the Cost is in the .tres file
    [Export] public Array<int> gold = new Array<int>();
    [Export] public Array<int> genes = new Array<int>();
}
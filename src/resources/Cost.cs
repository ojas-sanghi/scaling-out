using System.Collections.Generic;
using Godot;

public class Cost : Resource
{
    [Export] string statName = ""; // Not used but there so we can figure out what the Cost is in the .tres file
    [Export] public List<int> gold = new List<int>();
    [Export] public List<int> genes = new List<int>();
}
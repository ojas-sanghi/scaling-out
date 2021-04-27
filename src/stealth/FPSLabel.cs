using Godot;

public class FPSLabel : Label
{

    public override void _Process(float delta)
    {
        Text = Engine.GetFramesPerSecond().ToString();
    }

}
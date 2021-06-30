using Godot;

public class PostRoundMenu : Control
{
    void OnContinueConquestPressed()
    {
        Hide();
        GetTree().Paused = false;
    }
}
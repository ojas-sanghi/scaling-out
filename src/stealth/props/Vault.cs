using Godot;

public class Vault : Area2D
{

    // public override void _Ready()
    // {
    //     var animSprite = (AnimatedSprite)FindNode("AnimatedSprite");
    // }

    void OnVaultBodyEntered(Node body)
    {
        Events.publishLevelPassed();
    }

}
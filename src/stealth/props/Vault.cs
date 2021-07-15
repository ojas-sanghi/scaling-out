using Godot;

public class Vault : Area2D
{

    public override void _Ready()
    {
        var animSprite = (AnimatedSprite)FindNode("AnimatedSprite");
        // animSprite.Animation = StealthInfo.geneBeingPursued.ToString().ToLower();
    }

    void OnVaultBodyEntered(Node body)
    {
        Events.publishLevelPassed();
    }

}
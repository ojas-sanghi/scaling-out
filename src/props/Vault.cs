using Godot;

public class Vault : Area2D
{

    public override void _Ready()
    {
        var animSprite = (AnimatedSprite)FindNode("AnimatedSprite");

        // TODO: figure out a better way to organize
        if (StealthInfo.findingIce)
        {
            animSprite.Animation = "ice";
        }
        else
        {
            animSprite.Animation = "fire";
        }
    }

    void OnVaultBodyEntered(Node body)
    // TODO: figure out a better way to organize
    {
        if (StealthInfo.findingIce)
        {
            StealthInfo.findingIce = false;
            UpgradeInfo tankyInfo = DinoInfo.Instance.GetDinoInfo(Enums.Dinos.Tanky);
            // TODO: don't instantly upgrade a special, just give them genes
            tankyInfo.Upgrade(Enums.Stats.Special);
        }
        else
        {
            StealthInfo.findingFire = false;
            UpgradeInfo warriorInfo = DinoInfo.Instance.GetDinoInfo(Enums.Dinos.Warrior);
            // TODO: don't instantly upgrade a special, just give them genes
            warriorInfo.Upgrade(Enums.Stats.Special);
        }
        Events.publishLevelPassed();
    }

}
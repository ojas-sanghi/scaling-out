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

    //? Does this signal connection work?
    void _on_Vault_body_eneted(Node body)
    {
        if (StealthInfo.findingIce) {
            StealthInfo.findingIce = false;
            UpgradeInfo tankyInfo = DinoInfo.Instance.GetDinoInfo(Enums.Dinos.Tanky);
            tankyInfo.Upgrade(Enums.Stats.Special);
        } else {
            StealthInfo.findingFire = false;
            UpgradeInfo warriorInfo = DinoInfo.Instance.GetDinoInfo(Enums.Dinos.Warrior);
            warriorInfo.Upgrade(Enums.Stats.Special);
        }
        Events.publishLevelPassed();
    }

}
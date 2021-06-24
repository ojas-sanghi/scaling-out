using Godot;
using System;

public class ShopUnlockButton : Button
{
    bool canAfford = false;
    int goldCost;
    int genesCost;
    
    RichTextLabel richLabel;

    public override void _Ready()
    {
        // don't do anything if the dino is unlocked
        if (PlayerStats.Instance.dinosUnlocked.Contains(ShopInfo.shopDino))
        {
            return;
        }
        
        richLabel = GetNode<RichTextLabel>("RichTextLabel");
        
        var dinoUpgradeInfo = DinoInfo.Instance.GetDinoInfo(ShopInfo.shopDino);
        goldCost = dinoUpgradeInfo.unlockCostGold;
        genesCost = dinoUpgradeInfo.unlockCostGenes;

        canAfford = PlayerStats.gold >= goldCost && PlayerStats.genes >= genesCost;
        if (canAfford)
        {
            this.Disabled = false;
            richLabel.BbcodeText =
            $@"[center]Unlock
[img=50]res://assets/icons/coins.png[/img]{goldCost}
 [img=50]res://assets/icons/dna.png[/img]{genesCost} [/center]";
        }
        else
        {
            this.Disabled = true;
            richLabel.BbcodeText =
            $@"[center]Unlock
[color=red] [img=50]res://assets/icons/coins.png[/img]{goldCost}
[img=50]res://assets/icons/dna.png[/img]{genesCost} [/color] [/center]";
        }
    }

    void OnShopUnlockButtonPressed()
    {
        if (canAfford)
        {
            PlayerStats.gold -= goldCost;
            PlayerStats.genes -= genesCost;
            PlayerStats.Instance.dinosUnlocked.Add(ShopInfo.shopDino);
            PlayerStats.Instance.statsResource.SaveResource();
            Events.publishDinoUnlocked();
        }
    }

}

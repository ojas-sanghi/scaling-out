using Godot;

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
        Enums.Genes requiredGene = dinoUpgradeInfo.GetRequiredGene();

        bool hasGene = PlayerStats.Instance.genesFound.Contains(requiredGene);
        string geneString = requiredGene.ToString() + " gene";
        if (requiredGene == Enums.Genes.None)
        {
            hasGene = true;
            geneString = "";
        }

        canAfford = PlayerStats.gold >= goldCost && PlayerStats.genes >= genesCost && hasGene;
        if (canAfford)
        {
            this.Disabled = false;
            richLabel.BbcodeText =
            $@"[center]Unlock
[img=45]res://assets/icons/coins.png[/img]{goldCost}
[img=45]res://assets/icons/dna.png[/img]{genesCost}
{geneString}
 [/center]";
        }
        else
        {
            this.Disabled = true;
            richLabel.BbcodeText =
            $@"[center]Unlock
[color=red] [img=45]res://assets/icons/coins.png[/img]{goldCost}
[img=45]res://assets/icons/dna.png[/img]{genesCost}
{geneString}
 [/color]
 [/center]";



        }
    }

    void OnShopUnlockButtonPressed()
    {
        if (canAfford)
        {
            PlayerStats.gold -= goldCost;
            PlayerStats.genes -= genesCost;
            PlayerStats.Instance.AddDinoUnlocked(ShopInfo.shopDino);
            Events.publishDinoUnlocked();
        }
    }

}

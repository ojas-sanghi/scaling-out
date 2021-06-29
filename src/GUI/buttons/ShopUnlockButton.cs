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

        bool canAffordGold = PlayerStats.gold >= goldCost;
        bool canAffordGene = PlayerStats.genes >= genesCost;

        string goldColor = canAffordGold ? "white" : "red";
        string geneColor = canAffordGene ? "white" : "red";
        string specialGeneColor = hasGene ? "white" : "red";

        canAfford = canAffordGold && canAffordGene && hasGene;
        this.Disabled = !canAfford;
        richLabel.BbcodeText =
            $@"[center]Unlock
[img=45]res://assets/icons/coins.png[/img] [color={goldColor}] {goldCost} [/color]
[img=45]res://assets/icons/dna.png[/img] [color={genesCost}] {genesCost} [/color]
[color={specialGeneColor}] {geneString} [/color]
 [/center]";
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

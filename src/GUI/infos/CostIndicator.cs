using System;
using System.Collections.Generic;
using Godot;

public class CostIndicator : RichTextLabel
{
    [Export] NodePath parentNode;

    Enums.Stats buttonMode;

    int goldCost;
    int geneCost;


    public override void _Ready()
    {
        Events.dinoUpgraded += SetUpgradeCost;
    }

    public override void _ExitTree()
    {
        Events.dinoUpgraded -= SetUpgradeCost;
    }


    public override void _Process(float delta)
    {
        ShopUpgradeButton parentButton = GetNodeOrNull<ShopUpgradeButton>(parentNode);
        if (parentButton == null) return;

        if (parentButton.infoSet)
        {
            buttonMode = parentButton.statButtonMode;
            SetUpgradeCost();
        }
        SetProcess(false);
    }


    void SetUpgradeCost()
    {
        UpgradeInfo dinoInfo = DinoInfo.Instance.GetDinoInfo(ShopInfo.shopDino);

        if (dinoInfo.IsMaxedOut(buttonMode))
        {
            this.Hide();
        }

        List<int> cost = dinoInfo.GetNextUpgradeCost(buttonMode);
        goldCost = cost[0];
        geneCost = cost[1];

        string goldPic = "[img=<40>]res://assets/icons/coins.png[/img]";
        string genePic = "[img=<25>]res://assets/icons/dna.png[/img]";

        string goldText = goldCost.ToString();
        string geneText = geneCost.ToString();

        if (goldCost > ShopInfo.gold)
        {
            goldText = String.Format("[color=#ff0000] {0} [/color]", goldText);
        }
        if (geneCost > ShopInfo.genes)
        {
            geneText = String.Format("[color=#ff0000] {0} [/color]", geneText);
        }

        BbcodeText = String.Format("{0}{1}  {2}{3}", new string[] { goldPic, goldText, genePic, geneText });
    }
}
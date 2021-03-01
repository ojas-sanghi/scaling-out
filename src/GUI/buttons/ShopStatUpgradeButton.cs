using Godot;
using System.Collections.Generic;

using b = Enums.ShopUpgradeButtonModes;
using s = Enums.Stats;

public class ShopStatUpgradeButton : ShopUpgradeButton
{
    public override void SetButtonInfo()
    {
        switch (exportedButtonMode)
        {
            case b.Hp:
                //? font_color or FontColor
                stat.AddColorOverride("font_color", hpRed);
                statNum.AddColorOverride("font_color", hpRed);

                stat.Text = "HP";
                image.Texture = GD.Load<Texture>("res://assets/icons/heart.png");

                statButtonMode = s.Hp;
                break;
            
            case b.Delay:
                //? font_color or FontColor
                stat.AddColorOverride("font_color", delayBlue);
                statNum.AddColorOverride("font_color", delayBlue);

                stat.Text = "s";
                image.Texture = GD.Load<Texture>("res://assets/icons/timer.png");

                statButtonMode = s.Delay;
                break;
        }
        infoSet = true;

        var dinoInfo = DinoInfo.Instance.GetDinoInfo(ShopInfo.shopDino);
        
        statNum.Text = dinoInfo.GetStat(statButtonMode).ToString();
        List<int> cost = dinoInfo.GetNextUpgradeCost(statButtonMode);
        goldCost = cost[0];
        geneCost = cost[1];


    }

}
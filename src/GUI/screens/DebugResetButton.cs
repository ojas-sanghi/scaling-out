using Godot;
using System;

public class DebugResetButton : Button
{

    public void _on_DebugResetButton_pressed()
    {
        var upgradeInfo = DinoInfo.Instance.upgradesInfo[ShopInfo.shopDino];
        var data = upgradeInfo.data;
        foreach (Enums.Stats s in upgradeInfo.stats.Keys)
        {
            var stat = upgradeInfo.stats[s];
            stat.level = 0;
            data.SaveResource();
        }
    }

}

using System;
using Godot;

public class DebugResetALLButton : Button
{
    void _on_DebugResetALLButton_pressed()
    {
        var debugReset = new DebugResetButton();
        foreach (Enums.Dinos d in Enum.GetValues(typeof(Enums.Dinos)))
        {
            if (d == Enums.Dinos.None) continue;

            ShopInfo.shopDino = d;
            debugReset._on_DebugResetButton_pressed();
        }
    }

}

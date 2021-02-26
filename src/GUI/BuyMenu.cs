using Godot;

public class BuyMenu : Control
{
    CredCounter cred;
    SpinBox spinBox;

    public override void _Ready()
    {
        Hide();

        cred = (CredCounter)FindNode("CredCounter");
        spinBox = (SpinBox)FindNode("SpinBox");
    }

    //? does this work    
    void _on_BuyMenu_visbility_changed()
    {
        var bonusLabel = (Label)FindNode("ArmyElimBonus");

        UpdateMaxValue();
        cred.SetCreds();

        if (CombatInfo.Instance.numArmyKilled == 0)
        {
            bonusLabel.Text = "";
        }
        else
        {
            bonusLabel.Text = "Army elimination bonus: +" + (50 * CombatInfo.Instance.numArmyKilled).ToString();
        }
    }

    void UpdateMaxValue()
    {
        spinBox.MaxValue = CombatInfo.Instance.creds / DinoInfo.Instance.dinoCredCost;
    }

    void _on_Minus_pressed()
    {
        spinBox.Value--;
    }

    void _on_Plus_pressed()
    {
        spinBox.Value++;
    }

    void _on_Purchase_pressed()
    {
        Events.publishDinosPurchased((int)spinBox.Value);
        UpdateMaxValue();
        cred.SetCreds();
    }

    void _on_Conquest_pressed()
    {
        Hide();
        GetTree().Paused = false;
    }

}
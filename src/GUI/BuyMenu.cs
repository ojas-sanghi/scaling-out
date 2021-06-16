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

    void OnBuyMenuVisibilityChanged()
    {
        // TODO: make this a time bonus. se #119 https://app.gitkraken.com/glo/view/card/517b681f2d11497db53fc1bec843370a
        var bonusLabel = (Label)FindNode("ArmyElimBonus");

        UpdateMaxValue();
        cred.SetCreds();
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
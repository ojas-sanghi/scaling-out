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
        var bonusLabel = (Label)FindNode("ArmyElimBonus");

        UpdateMaxValue();
        cred.UpdateCreds();
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

    void OnPurchasePressed()
    {
        Events.publishDinosPurchased((int)spinBox.Value);
        UpdateMaxValue();
        cred.UpdateCreds();
    }

    async void OnConquestPressed()
    {
        Hide();
        GetTree().Paused = false;
        await SceneChanger.Instance.NextCombatRound();
    }

}
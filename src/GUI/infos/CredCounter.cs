using Godot;

public class CredCounter : Control
{

    public override void _Ready()
    {
        Events.dinosPurchased += OnDinosPurchased;

        UpdateCreds();
    }

    public override void _ExitTree()
    {
        Events.dinosPurchased -= OnDinosPurchased;
    }

    public void UpdateCreds()
    {
        var label = (Label)FindNode("Label");
        label.Text = CombatInfo.Instance.creds.ToString();
    }

    void OnDinosPurchased(int num)
    {
        UpdateCreds();
    }


}
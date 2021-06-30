using Godot;

public class CredCounter : Control
{

    public override void _Ready()
    {
        Events.dinoDeployed += OnDinoDeployed;
        Events.newRound += UpdateCreds;
        UpdateCreds();
    }

    public void UpdateCreds()
    {
        var label = (Label)FindNode("Label");
        label.Text = CombatInfo.Instance.creds.ToString();
    }

    void OnDinoDeployed(Enums.Dinos dinos)
    {
        UpdateCreds();
    }

}
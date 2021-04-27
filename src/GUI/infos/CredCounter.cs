using Godot;

public class CredCounter : Control
{

    public override void _Ready()
    {
        SetCreds();
    }

    public void SetCreds()
    {
        var label = (Label)FindNode("Label");
        label.Text = CombatInfo.Instance.creds.ToString();
    }

}
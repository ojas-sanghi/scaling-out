using Godot;

public class RoundCounter : Control
{

    public override void _Ready()
    {
        Events.roundWon += SetText;

        SetText();
    }

    public override void _ExitTree()
    {
        Events.roundWon -= SetText;
    }

    void SetText()
    {
        CombatInfo c = CombatInfo.Instance;
        Label label = GetNode<Label>("Label");

        label.Text = "Round: " + c.currentRound.ToString() + " / " + c.maxRounds.ToString();
    }

}
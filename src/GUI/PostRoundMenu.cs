using Godot;

public class PostRoundMenu : Control
{
    public override void _Ready()
    {
        Hide();
    }

    void OnPostRoundMenuVisibilityChanged()
    {
        Combat combatScreen = (Combat)GetParent();

        Label roundBonus = (Label)FindNode("WinBonus");
        Label timeBonus = (Label)FindNode("TimeBonus");
        Label totalBonus = (Label)FindNode("TotalBonus");

        roundBonus.Text = $"Round win bonus: +{combatScreen.roundBonusCreds}";
        timeBonus.Text = $"Time bonus: +{combatScreen.timeBonusCreds}";
        totalBonus.Text = $"Total bonus: {combatScreen.roundBonusCreds + combatScreen.timeBonusCreds}";
    }

    void OnContinueConquestPressed()
    {
        Hide();
        GetTree().Paused = false;
    }
}
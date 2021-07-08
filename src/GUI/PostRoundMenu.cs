using Godot;

public class PostRoundMenu : Control
{
    public override void _Ready()
    {
        Hide();

        // Hide when a new round is triggered from scene changer
        // dont hide when button is pressed since otherwise that looks weird
        Events.newRound += Hide;
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
        GetTree().Paused = false;
    }
}
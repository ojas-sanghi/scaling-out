using Godot;

public class DinoCounter : Control
{
    public override void _Ready()
    {
        Events.dinoDeployed += OnDinoDeployed;
        Events.newRound += UpdateText;
        UpdateText();
    }

    public override void _ExitTree()
    {
        Events.dinoDeployed -= OnDinoDeployed;
        Events.newRound -= UpdateText;
    }

    void OnDinoDeployed()
    {
        CombatInfo.Instance.dinosRemaining--;
        if (CombatInfo.Instance.dinosRemaining == 0)
        {
            Events.publishAllDinosExpended();
        }
        UpdateText();
    }

    void UpdateText()
    {
        string labelText = "Remaining dinos: " + CombatInfo.Instance.dinosRemaining.ToString();
        GetNode<Label>("Label").Text = labelText;
    }
}
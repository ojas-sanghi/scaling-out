using Godot;

public class DinoCounter : Control
{
    public override void _Ready()
    {
        Events.dinoDeployed += OnDinoDeployed;
        Events.dinosPurchased += OnDinosPurchased;
        Events.newRound += UpdateText;
        UpdateText();
    }

    public override void _ExitTree()
    {
        Events.dinoDeployed -= OnDinoDeployed;
        Events.dinosPurchased -= OnDinosPurchased;
        Events.newRound -= UpdateText;
    }

    void OnDinoDeployed(Enums.Dinos dinoType)
    {
        CombatInfo.Instance.dinosRemaining--;
        if (CombatInfo.Instance.dinosRemaining <= 0)
        {
            Events.publishAllDinosExpended();
        }
        UpdateText();
    }

    async void UpdateText()
    {   
        // Wait for 0.1 seconds to allow all the signals connected to this to be registered and executed elsewhere
        // Only once that code executes will our un-fading code work properly
        // Kinda hacky but oh well
        await ToSignal(GetTree().CreateTimer(0.2f), "timeout");

        GD.Print("called :))");

        string labelText = "Remaining dinos: " + CombatInfo.Instance.dinosRemaining.ToString();
        GetNode<Label>("Label").Text = labelText;
    }

    void OnDinosPurchased(int num)
    {
        UpdateText();
    }
}
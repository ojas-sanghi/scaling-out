using Godot;

public class DNACounter : Control
{
    public override void _Ready()
    {
        Events.dinoUpgraded += UpdateGeneAmt;
        Events.dinoUnlocked += UpdateGeneAmt;
        UpdateGeneAmt();
    }

    public override void _ExitTree()
    {
        Events.dinoUpgraded -= UpdateGeneAmt;
        Events.dinoUnlocked -= UpdateGeneAmt;
    }

    void UpdateGeneAmt()
    {
        Label label = (Label)FindNode("Label");
        label.Text = PlayerStats.genes.ToString();
    }

}

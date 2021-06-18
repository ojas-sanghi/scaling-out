using Godot;

public class DNACounter : Control
{
    public override void _Ready()
    {
        Events.dinoUpgraded += UpdateGeneAmt;
        UpdateGeneAmt();
    }

    public override void _ExitTree()
    {
        Events.dinoUpgraded -= UpdateGeneAmt;
    }

    void UpdateGeneAmt()
    {
        Label label = (Label)FindNode("Label");
        label.Text = PlayerStats.genes.ToString();
    }

}

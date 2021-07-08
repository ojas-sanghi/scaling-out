using Godot;

public class GeneStealthMap : Node
{
    [Export] Enums.Genes rewardGene;

    public override void _Ready()
    {
        if (rewardGene == Enums.Genes.None)
        {
            GD.PushError("You must set rewardGene for GeneStealthMap!");
            GD.PrintStack();
            GetTree().Quit(1);
        }

        Events.levelPassed += OnLevelPassed;
    }

    void OnLevelPassed()
    {
        PlayerStats.genes += 150; // TODO: store the rewards for this in some config file and draw from there

        PlayerStats.Instance.AddGeneFound(rewardGene);
    }
}
using Godot;

public class GeneStealthMap : StealthMap
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
        difficulty = Enums.StealthMapDifficultyLevel.Hard;

        base._Ready();
    }

    public override void OnLevelPassed()
    {
        PlayerStats.Instance.AddGeneFound(rewardGene);
        Events.publishGeneFound();
        randomGeneChance = false;

        base.OnLevelPassed();
    }
}
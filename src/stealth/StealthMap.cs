using Godot;

public class StealthMap : Node
{
    [Export] protected Enums.StealthMapDifficultyLevel difficulty;

    public override void _Ready()
    {
        if (difficulty == Enums.StealthMapDifficultyLevel.None)
        {
            GD.PushError("You must set difficulty for StealthMap!");
            GD.PrintStack();
            GetTree().Quit(1);
        }

        Events.levelPassed += OnLevelPassed;
    }

    public virtual void OnLevelPassed()
    {
        PlayerStats.genes += StealthInfo.Instance.difficultyGeneRewards[difficulty];
    }
}
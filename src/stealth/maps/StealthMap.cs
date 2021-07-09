using Godot;
using System.Collections.Generic;

public class StealthMap : Node
{
    [Export] protected Enums.StealthMapDifficultyLevel difficulty;

    // whether or not player has a chance of getting a random gene after finishing the map
    // turned off by the gene stealth maps when those are active
    protected bool randomGeneChance = true;

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
        StealthInfo s = StealthInfo.Instance;

        PlayerStats.genes += s.difficultyGeneRewards[difficulty];

        // now check if the player got lucky and gets a random gene
        // but if the flag is off, then quit
        if (!randomGeneChance) return;
        if (StealthInfo.rng.NextDouble() < StealthInfo.geneFindChance)
        {
            List<Enums.Genes> notFoundGenes = s.GetNotFoundGenes();
            if (notFoundGenes.Count > 0)
            {
                int index = StealthInfo.rng.Next(notFoundGenes.Count);
                PlayerStats.Instance.AddGeneFound(notFoundGenes[index]);
                Events.publishGeneFound();
                GD.Print("YOU FOUND A RANDOM GENE!!!!!!!11");
            }
        }
    }
}
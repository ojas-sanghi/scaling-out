using Godot;

public class Combat : Node
{
    [Export] int maxRounds = 3;
    [Export] int roundLengthSeconds = 120;

    CombatInfo c = CombatInfo.Instance;

    public override void _Ready()
    {
        c.Reset(maxRounds);
        c.currentRound = 1;

        //! Note I have no idea what the next comments are, they just are
        // can't put that in reset() since otherwise it would do that between rounds
        // note: this executes AFTER RoundCounter grabs the data, so...
        //	CombatInfo.current_round = 3

        GameTimer gameTimer = (GameTimer)FindNode("GameTimer");
        gameTimer.timerDuration = roundLengthSeconds;
        gameTimer.StartTimer();

        Events.roundWon += OnRoundWon;
        Events.newRound += OnNewRound;

        Events.conquestLost += OnConquestLost;
        Events.conquestWon += OnConquestWon;
    }

    public override void _ExitTree()
    {
        Events.roundWon -= OnRoundWon;
        Events.newRound -= OnNewRound;

        Events.conquestLost -= OnConquestLost;
        Events.conquestWon -= OnConquestWon;
    }

    void OnRoundWon()
    {
        Events.roundWon -= OnRoundWon;

        c.currentRound++;
        c.creds += 150; // TODO: extract this to a singleton (once we figure out post-round credit granting mechanic)

        if (c.currentRound > c.maxRounds)
        {
            Events.publishConquestWon();
            return;
        }

        GetTree().Paused = true;
        GetNode<PostRoundMenu>("PostRoundMenu").Show();
    }

    // When a new round starts, re-connect the won round signal
    void OnNewRound()
    {
        Events.roundWon += OnRoundWon;
    }

    void OnConquestLost()
    {
        SceneChanger.Instance.GoToScene("res://src/GUI/dialogues/CombatLoseDialogue.tscn");
    }

    void OnConquestWon()
    {
        SceneChanger.Instance.GoToScene("res://src/GUI/dialogues/CombatWinDialogue.tscn");

        PlayerStats.gold += CityInfo.Instance.currentCity.rewardGold;
    }
}

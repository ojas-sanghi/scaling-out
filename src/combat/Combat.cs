using Godot;

public class Combat : Node
{
    int maxRounds = CityInfo.Instance.currentCity.rounds;

    CombatInfo c = CombatInfo.Instance;

    public CombatTimer CombatTimer;
    public int roundBonusCreds;
    public int timeBonusCreds;

    public override void _Ready()
    {
        int startingCreds = CityInfo.Instance.currentCity.roundWinCreditBonus[0];
        c.Reset(startingCreds, maxRounds);

        //! Note I have no idea what the next comments are, they just are
        // can't put that in reset() since otherwise it would do that between rounds
        // note: this executes AFTER RoundCounter grabs the data, so...
        //	CombatInfo.current_round = 3

        CombatTimer = (CombatTimer)FindNode("CombatTimer");

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
        if (c.currentRound > c.maxRounds)
        {
            Events.publishConquestWon();
            return;
        }

        roundBonusCreds = CityInfo.Instance.currentCity.roundWinCreditBonus[c.currentRound - 1];
        timeBonusCreds = (int)Mathf.Round(CombatTimer.timer.TimeLeft); // 1 second left = 1 more credit

        c.creds += roundBonusCreds + timeBonusCreds;

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

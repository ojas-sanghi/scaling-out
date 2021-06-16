using Godot;

public class Combat : Node
{
    [Export] int maxDinos = 10;
    [Export] int rewardGold = 100;
    [Export] int maxRounds = 3;

    CombatInfo c = CombatInfo.Instance;

    public override void _Ready()
    {
        c.Reset(maxDinos, maxRounds);

        //! Note I have no idea what the next comments are, they just are
        // can't put that in reset() since otherwise it would do that between rounds
        // note: this executes AFTER RoundCounter grabs the data, so...
        //	CombatInfo.current_round = 3

        Events.roundWon += OnRoundWon;
        Events.newRound += OnNewRound;

        Events.conquestLost += OnConquestLost;
        Events.conquestWon += OnConquestWon;
        Events.dinosPurchased += OnDinosPurchased;
    }

    public override void _ExitTree()
    {
        Events.roundWon -= OnRoundWon;
        Events.newRound -= OnNewRound;

        Events.conquestLost -= OnConquestLost;
        Events.conquestWon -= OnConquestWon;
        Events.dinosPurchased -= OnDinosPurchased;
    }

    void OnRoundWon()
    {
        Events.roundWon -= OnRoundWon;

        c.currentRound += 1;
        c.creds += 150; // TODO: extract this to a singleton

        // set the remaining to the max
        // the dinos that are bought will be added to the max later
        maxDinos = c.dinosRemaining;
        c.Reset(maxDinos);

        if (c.currentRound > c.maxRounds)
        {
            Events.publishConquestWon();
            return;
        }

        GetTree().Paused = true;
        GetNode<BuyMenu>("BuyMenu").Show();
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
        ShopInfo.gold += rewardGold;
    }

    void OnDinosPurchased(int num)
    {
        maxDinos += num;
        c.creds -= num * DinoInfo.Instance.dinoCredCost;
        c.Reset(maxDinos);
    }

}

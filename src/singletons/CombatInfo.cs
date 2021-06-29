using System.Collections.Generic;
using Godot;
using Godot.Collections;

/*
  Holds conquest/round-specific info
  Reset between conquests
*/
public class CombatInfo : Node
{
    public static CombatInfo Instance;

    public int maxDinos;
    public int dinosRemaining;
    public int dinosDied;

    public Enums.Dinos selectedDinoType;

    public List<Enums.Dinos> dinosDeploying = new List<Enums.Dinos>();
    public List<Enums.Dinos> selectorTimerList = new List<Enums.Dinos>();

    public List<Enums.SpecialAbilities> abilitiesUsed = new List<Enums.SpecialAbilities>(); 

    public List<Lane> lanesInDanger = new List<Lane>();

    public int currentRound = 1;
    public int maxRounds = 3;

    public int creds = 100;

    public CombatInfo()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Reset();

        Instance = this;
        Events.dinoDeployed += OnDinoDeployed;
    }

    public override void _ExitTree()
    {
        Events.dinoDeployed -= OnDinoDeployed;
    }

    public void Reset(int _MaxDinos = 10, int _MaxRounds = 3)
    {
        maxDinos = _MaxDinos;
        dinosRemaining = _MaxDinos;
        dinosDied = 0;

        selectedDinoType = Enums.Dinos.Mega;

        dinosDeploying.Clear();
        selectorTimerList.Clear();

        abilitiesUsed.Clear();
        lanesInDanger.Clear();

        maxRounds = _MaxRounds;
    }

    public bool IsAbilityDeployable(Enums.Dinos dinoType)
    {
        DinoInfo d = DinoInfo.Instance;

        var dinosLeft = GetTree().GetNodesInGroup("dinos");
        bool specificDinoTypeLeft = false;
        foreach (BaseDino baseDino in dinosLeft)
        {
            if (baseDino.dinoType == dinoType) specificDinoTypeLeft = true;
        }

        // check for 3 things
        // 1. there are dinos of dinoType left that are still alive
        // 2. user has unlocked the special ability
        // 3. the associated ability for dinoType has not been used yet
        return specificDinoTypeLeft && d.GetDinoInfo(dinoType).UnlockedSpecial() && !abilitiesUsed.Contains(DinoInfo.Instance.dinoTypesAndAbilities[dinoType]);
    }

    void OnDinoDeployed(Enums.Dinos dinoType)
    {
        // Add to list of dinos just deployed
        // Prevents this type from being deployed until cooldown finished
        dinosDeploying.Add(dinoType);

        // Wait for the dino-specific delay
        float delay = DinoInfo.Instance.GetDinoTimerDelay(dinoType);
        // await ToSignal(GetTree().CreateTimer(delay), "timeout");

        // // remove dino from list
        // dinosDeploying.Remove(dinoId);

        Timer dinosDeployingTimer = new Timer();
        dinosDeployingTimer.OneShot = true;
        AddChild(dinosDeployingTimer);

        dinosDeployingTimer.Connect("timeout", this, "OnDinosDeployingTimerTimeout", new Array(new Enums.Dinos[] { dinoType }));
        dinosDeployingTimer.Start(delay);
    }

    void OnDinosDeployingTimerTimeout(Enums.Dinos dinoType)
    {
        dinosDeploying.Remove(dinoType);
    }

}
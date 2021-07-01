using System.Collections.Generic;
using Godot;

/*
  Holds conquest/round-specific info
  Reset between conquests
*/
public class CombatInfo : Node
{
    public static CombatInfo Instance;

    public Enums.Dinos selectedDinoType;

    public List<Enums.Dinos> dinosDeploying = new List<Enums.Dinos>();
    public List<Enums.Dinos> selectorTimerList = new List<Enums.Dinos>();

    public List<Enums.SpecialAbilities> abilitiesUsed = new List<Enums.SpecialAbilities>();

    public List<Lane> lanesInDanger = new List<Lane>();

    public int currentRound;
    public int maxRounds;

    public int creds;

    public bool allMoneyExpended = false;

    public CombatInfo()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Reset();

        Instance = this;

        Events.dinoDiedType += OnDinoDiedType;
    }

    public override void _ExitTree()
    {
        Events.dinoDiedType -= OnDinoDiedType;
    }

    public void Reset()
    {
        CityInfoResource currentCity = CityInfo.Instance.currentCity;

        selectedDinoType = Enums.Dinos.Mega;

        dinosDeploying.Clear();
        selectorTimerList.Clear();
        abilitiesUsed.Clear();
        lanesInDanger.Clear();

        currentRound = 1;
        maxRounds = currentCity.rounds;

        creds = currentCity.roundWinCreditBonus[0];

        allMoneyExpended = false;
    }

    void OnDinoDiedType(Enums.Dinos dino)
    {
        // go through each dino and check if we can afford them
        // if we can afford even one, then we haven't lost yet, and we quit the function
        // otherwise, if we can't afford any, then set allMoneyExpended to true
        foreach (Enums.Dinos d in PlayerStats.Instance.dinosUnlocked)
        {
            if (DinoInfo.Instance.CanAffordDino(d))
                return;
            allMoneyExpended = true;       
        }

        // note: the logic for numDinosLeft is done in BaseDino.cs so that it happens after the dino fades away
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
}
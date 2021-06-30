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

    public Enums.Dinos selectedDinoType;

    public List<Enums.Dinos> dinosDeploying = new List<Enums.Dinos>();
    public List<Enums.Dinos> selectorTimerList = new List<Enums.Dinos>();

    public List<Enums.SpecialAbilities> abilitiesUsed = new List<Enums.SpecialAbilities>(); 

    public List<Lane> lanesInDanger = new List<Lane>();

    public int currentRound = 1;
    public int maxRounds = 3;

    public int creds = 200;

    public CombatInfo()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Reset();

        Instance = this;
    }

    public void Reset(int _MaxRounds = 3)
    {
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
}
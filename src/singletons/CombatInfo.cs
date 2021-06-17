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

    public Enums.Dinos dinoId;

    public List<Enums.Dinos> dinosDeploying = new List<Enums.Dinos>();
    public List<Enums.Dinos> selectorTimerList = new List<Enums.Dinos>();

    public bool shotIce;
    public bool shotFire;

    public int currentRound = 1;
    public int maxRounds = 3;

    public int creds = 100;

    // TODO: Remove this from here and place it somewhere it makes sense
    public int bulletSpeed = 400;

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

        dinoId = Enums.Dinos.Tanky;

        dinosDeploying.Clear();
        selectorTimerList.Clear();

        shotIce = false;
        shotFire = false;

        maxRounds = _MaxRounds;
    }

    public bool IsAbilityDeployable(Enums.Dinos dinoType)
    {
        DinoInfo d = DinoInfo.Instance;

        if (dinoType == Enums.Dinos.Tanky)
        {
            return d.GetDinoInfo(dinoType).UnlockedSpecial() && !this.shotIce;
        }
        else if (dinoType == Enums.Dinos.Warrior)
        {
            return d.GetDinoInfo(dinoType).UnlockedSpecial() && !this.shotFire;
        }
        else
        {
            return false;
        }
    }

    void OnDinoDeployed()
    {
        // Add to list of dinos just deployed
        // Prevents this type from being deployed until cooldown finished
        dinosDeploying.Add(dinoId);

        // Wait for the dino-specific delay
        double delay = DinoInfo.Instance.GetDinoTimerDelay();
        // await ToSignal(GetTree().CreateTimer((float)delay), "timeout");

        // // remove dino from list
        // dinosDeploying.Remove(dinoId);

        Timer dinosDeployingTimer = new Timer();
        dinosDeployingTimer.OneShot = true;
        AddChild(dinosDeployingTimer);

        dinosDeployingTimer.Connect("timeout", this, "OnDinosDeployingTimerTimeout", new Array(new Enums.Dinos[] { dinoId }));
        dinosDeployingTimer.Start((float)delay);
    }

    void OnDinosDeployingTimerTimeout(Enums.Dinos id)
    {
        dinosDeploying.Remove(id);
    }

}
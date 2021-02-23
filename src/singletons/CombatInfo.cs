using System.Collections.Generic;
using Godot;
using Godot.Collections;

/*
* Holds conquest/round-specific info
Placed in a singleton for ease of access
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
    public List<int> selectorTimerList = new List<int>();

    public bool shotIce;
    public bool shotFire;

    // Number of army people killed; you get a cred bonus for each
    public int numArmyKilled;

    public int currentRound;
    public int maxRounds;

    public int creds;

    // TODO: Remove this from here and place it somewhere it makes sense
    public int bulletSpeed;

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

    public void Reset(int _MaxDinos = 10, int _MaxRounds = 3) {
        maxDinos = _MaxDinos;
        dinosRemaining = _MaxDinos;
        dinosDied = 0;

        dinoId = Enums.Dinos.Tanky;

        dinosDeploying.Clear();
        selectorTimerList.Clear();

        shotIce = false;
        shotFire = false;

        numArmyKilled = 0;

        maxRounds = _MaxRounds;
    }

    void OnDinoDeployed() {
        // Add to list of dinos just deployed
        // Prevents this type from being deployed until cooldown finished
        dinosDeploying.Add(dinoId);

        // Wait for the dino-specific delay
        double delay = DinoInfo.Instance.GetDinoTimerDelay();
        Timer dinosDeployingTimer = new Timer();
        dinosDeployingTimer.OneShot = true;
        AddChild(dinosDeployingTimer);

        dinosDeployingTimer.Connect("timeout", this, "OnDinosDeployingTimerTimeout", new Array(dinoId));
        dinosDeployingTimer.Start((float) delay);
    }

    void OnDinosDeployingTimerTimeout(Enums.Dinos id) {
        dinosDeploying.Remove(id);
    }

}
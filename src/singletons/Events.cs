using System;
using Godot;

public class Events : Node
{
    public static event Action<Enums.Dinos> dinoDeployed;
    public static event Action<Enums.Dinos> dinoFullySpawned;
    public static event Action<Enums.Dinos> dinoDiedType; // same signal, but different parameter for diff. situations
    public static event Action<BaseDino> dinoDiedInstance; // same signal, but different parameter for diff. situations

    public static event Action<double> dinoHit;
    public static event Action<float> blockadeHit;

    public static event Action roundWon; //! Connect oneshot in _ready, re-connect when newRound
    public static event Action newRound;
    public static event Action conquestWon;
    public static event Action conquestLost;

    public static event Action<int> dinosPurchased; // this is during combat
    public static event Action dinoUnlocked; // this is in the shop

    public static event Action<int> coinGrabbed;

    public static event Action scientistEnteredWarnZone;
    public static event Action<Vector2> scientistEnteredCameraZone;
    public static event Action levelPassed;
    public static event Action levelFailed; //! Connect oneshot in _ready, re-connect when newRound

    public static event Action geneFound; // TODO: cool vfx when this is found

    public static event Action<SelectorSprite> selectorSelected;
    public static event Action<Enums.SpecialAbilities> projectileHit;

    public static event Action dinoUpgraded;


    ///////////////////////////////////

    public static void publishDinoDeployed(Enums.Dinos type) => dinoDeployed?.Invoke(type);
    public static void publishDinoFullySpawned(Enums.Dinos type) => dinoFullySpawned?.Invoke(type);
    public static void publishDinoDiedType(Enums.Dinos type) => dinoDiedType?.Invoke(type);
    public static void publishDinoDiedInstance(BaseDino dino) => dinoDiedInstance?.Invoke(dino);

    public static void publishDinoHit(double damage) => dinoHit?.Invoke(damage);
    public static void publishBlockadeHit(float damage) => blockadeHit?.Invoke(damage);

    public static void publishRoundWon() => roundWon?.Invoke();
    public static void publishNewRound() => newRound?.Invoke();
    public static void publishConquestWon() => conquestWon?.Invoke();
    public static void publishConquestLost() => conquestLost?.Invoke();

    public static void publishDinosPurchased(int num) => dinosPurchased?.Invoke(num);
    public static void publishDinoUnlocked() => dinoUnlocked?.Invoke();

    public static void publishCoinGrabbed(int num) => coinGrabbed?.Invoke(num);

    public static void publishScientistEnteredWarnZone() => scientistEnteredWarnZone?.Invoke();
    public static void publishScientistEnteredCameraZone(Vector2 pos) => scientistEnteredCameraZone?.Invoke(pos);
    public static void publishLevelPassed() => levelPassed?.Invoke();
    public static void publishLevelFailed() => levelFailed?.Invoke();

    public static void publishGeneFound() => geneFound?.Invoke();

    public static void publishSelectorSelected(SelectorSprite selector) => selectorSelected?.Invoke(selector);
    public static void publishProjectileHit(Enums.SpecialAbilities type) => projectileHit?.Invoke(type);

    public static void publishDinoUpgraded() => dinoUpgraded?.Invoke();


    public override void _Ready()
    {
        OS.WindowMaximized = true;

        
        int releaseVolumeDb = -5;
        int debugVolumeDb = -80;
        var busIndex = AudioServer.GetBusIndex("Master");
        if (OS.IsDebugBuild())
        {
            AudioServer.SetBusVolumeDb(busIndex, debugVolumeDb);
        }
        else
        {
            AudioServer.SetBusVolumeDb(busIndex, releaseVolumeDb);
        }
    }
}
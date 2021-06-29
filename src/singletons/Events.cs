using System;
using Godot;

public class Events : Node
{
    public static event Action<Enums.Dinos> dinoDeployed;
    public static event Action<Enums.Dinos> dinoFullySpawned;
    public static event Action<Enums.Dinos> dinoDied;

    public static event Action<double> dinoHit;
    public static event Action<float> blockadeHit;

    public static event Action newRound;
    public static event Action roundWon; //! Connect oneshot in _ready, re-connect when newRound
    public static event Action conquestWon;
    public static event Action conquestLost;

    public static event Action<int> dinosPurchased; // this is during combat
    public static event Action dinoUnlocked; // this is in the shop

    public static event Action<int> coinGrabbed;

    public static event Action levelPassed;
    public static event Action levelFailed; //! Connect oneshot in _ready, re-connect when newRound

    public static event Action<Enums.Genes> projectileHit;

    public static event Action allDinosExpended;

    public static event Action dinoUpgraded;

    public static event Action<SelectorSprite> selectorSelected;

    ///////////////////////////////////

    public static void publishDinoDeployed(Enums.Dinos type) => dinoDeployed?.Invoke(type);
    public static void publishDinoFullySpawned(Enums.Dinos type) => dinoFullySpawned?.Invoke(type);
    public static void publishDinoDied(Enums.Dinos type) => dinoDied?.Invoke(type);

    public static void publishDinoHit(double damage) => dinoHit?.Invoke(damage);
    public static void publishBlockadeHit(float damage) => blockadeHit?.Invoke(damage);

    public static void publishNewRound() => newRound?.Invoke();
    public static void publishroundWon() => roundWon?.Invoke();
    public static void publishConquestWon() => conquestWon?.Invoke();
    public static void publishConquestLost() => conquestLost?.Invoke();

    public static void publishDinosPurchased(int num) => dinosPurchased?.Invoke(num);
    public static void publishDinoUnlocked() => dinoUnlocked?.Invoke();

    public static void publishCoinGrabbed(int num) => coinGrabbed?.Invoke(num);

    public static void publishLevelPassed() => levelPassed?.Invoke();
    public static void publishLevelFailed() => levelFailed?.Invoke();

    public static void publishProjectileHit(Enums.Genes type) => projectileHit?.Invoke(type);

    public static void publishAllDinosExpended() => allDinosExpended?.Invoke();

    public static void publishDinoUpgraded() => dinoUpgraded?.Invoke();

    public static void publishSelectorSelected(SelectorSprite selector) => selectorSelected?.Invoke(selector);

    public override void _Ready()
    {
        OS.WindowMaximized = true;
    }
}
using System;

public class Events {

    // public delegate void GameBuildingPlaced(string buildingId, int playerNum, GameBuildingType type, Vector2 position);
    // public static event GameBuildingPlaced GameBuildingPlacedEvent;

    public static event Action dinoDeployed;
    public static event Action dinoFullySpawned;
    public static event Action dinoDied;
    
    public static event Action dinoHit;
    public static event Action blockadeHit;

    public static event Action newRound;
    public static event Action roundWon;
    public static event Action conquestWon;
    public static event Action conquestLost;

    public static event Action dinosPurchased;

    public static event Action coinGrabbed;

    public static event Action levelPassed;
    public static event Action levelFailed;

    public static event Action projectileHit;

    public static event Action allDinosExpended;

    public static event Action dinoUpgraded;

    public static void publishDinoHit() {
        dinoHit?.Invoke();
    }

}
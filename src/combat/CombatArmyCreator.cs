using Godot;
using System.Collections.Generic;

public class CombatArmyCreator : Node2D
{
    PackedScene armyZone;
    Area2D deployArea;
    RectangleShape2D deployShape;

    float armyZoneXSize = 200f;
    float armyZoneYSize; // taken from LanesCreator
    [Export] public float armyZoneYPadding = 130f; // how much is subtracted from the y dimension (vertical side) for padding from the lane's edges

    float armyZoneXPos; // retrived from DeployArea
    float armyZoneYPos; // zone number * armyZoneYSize

    List<CombatArmyZone> armyZones = new List<CombatArmyZone>();
    CombatArmyZone zoneLastDeployedTo;
    int soldierNum = 0;

    public override void _Ready()
    {
        armyZone = GD.Load<PackedScene>("res://src/combat/CombatArmyZone.tscn");
        deployArea = GetNode<Area2D>("DeployArea");
        armyZoneXPos = deployArea.Position.x;

        LanesCreator lanesCreator = GetParent().GetNode<LanesCreator>("LanesCreator");
        armyZoneYSize = lanesCreator.yPixelsForEachLane;
        int numLanes = lanesCreator.numLanes;

        // step-by-step
        // 1. get the associated lane with the zone AND the lane's danger zone
        // 2. make the extents of the new army zone -- the predefined X size, and the same Y size as the danger zone
        // 3. pass that into the army zone, which just sets itself to that
        // 4. get and set the position of the new army zone -- the predefined x position, and the same Y position as the danger zone
        for (int i = 0; i < numLanes; i++)
        {
            Lane associatedLane = lanesCreator.lanes[i];
            CollisionShape2D laneDangerZoneCollision = associatedLane.dangerZoneCollision;
            
            float newZoneExtentsX = armyZoneXSize / 2;
            float newZoneExtentsY = ((RectangleShape2D)laneDangerZoneCollision.Shape).Extents.y;

            var newZone = armyZone.Instance<CombatArmyZone>();
            newZone.areaExtents = new Vector2(newZoneExtentsX, newZoneExtentsY - (armyZoneYPadding / 2));
            newZone.Position = new Vector2(armyZoneXPos, laneDangerZoneCollision.GlobalPosition.y);

            armyZones.Add(newZone);
            AddChild(newZone);
        }

        Events.newRound += MakeArmySoldiers;

        MakeArmySoldiers();
    }

    public override void _ExitTree()
    {
        Events.newRound -= MakeArmySoldiers;
    }

    void MakeArmySoldiers()
    {
        CityInfoResource currentCity = CityInfo.Instance.currentCity;

        // make the amount of soldiers specified for this round
        int newSoldiersThisRound = currentCity.numSoldiersPerRound[CombatInfo.Instance.currentRound - 1];
        for (int i = 0; i < newSoldiersThisRound; i++)
        {
            Enums.ArmyGunTypes gunType = currentCity.soldierGunTypes[soldierNum];
            int zoneIndex = currentCity.soldierZoneIndex[soldierNum];
            CombatArmyZone zoneToDeployTo = armyZones[zoneIndex];

            zoneToDeployTo.SummonArmySoldier(soldierNum, gunType);
            soldierNum++;
            zoneLastDeployedTo = zoneToDeployTo;
        }
    }
}

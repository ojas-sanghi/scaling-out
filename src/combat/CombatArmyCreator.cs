using Godot;
using System.Collections.Generic;

public class CombatArmyCreator : Node2D
{
    PackedScene armyZone;
    Area2D deployArea;
    RectangleShape2D deployShape;

    float armyZoneXSize = 200f;
    float armyZoneYSize; // taken from LanesCreator
    [Export] public float armyZoneYPadding = 100f; // how much is subtracted from the y dimension (vertical side) for padding from the lane's edges

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

        for (int i = 0; i < numLanes; i++)
        {
            var newZone = armyZone.Instance<CombatArmyZone>();
            // note: we don't divide these earlier since ySize is used to get the yPos
            Vector2 areaExtents = new Vector2(armyZoneXSize / 2, (armyZoneYSize - armyZoneYPadding) / 2);
            newZone.areaExtents = areaExtents;

            armyZoneYPos = armyZoneYSize * (i + 1);
            newZone.Position = new Vector2(armyZoneXPos, armyZoneYPos);
            armyZones.Add(newZone);

            AddChild(newZone);
        }

        Events.newRound += MakeArmySoliders;

        MakeArmySoliders();
    }

    public override void _ExitTree()
    {
        Events.newRound -= MakeArmySoliders;
    }

    void MakeArmySoliders()
    {
        CityInfoResource currentCity = CityInfo.Instance.currentCity;

        // make the amount of soliders specified for this round
        int newSolidersThisRound = currentCity.numSoldiersPerRound[CombatInfo.Instance.currentRound - 1];
        for (int i = 0; i < newSolidersThisRound; i++)
        {
            Enums.ArmyGunTypes gunType = currentCity.soldierGunTypes[soldierNum];
            int zoneIndex = currentCity.soliderZoneIndex[soldierNum];
            CombatArmyZone zoneToDeployTo = armyZones[zoneIndex];

            zoneToDeployTo.SummonArmySolider(soldierNum, gunType);
            soldierNum++;
            zoneLastDeployedTo = zoneToDeployTo;
        }
    }
}

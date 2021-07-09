using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CombatArmyZone : Area2D
{
    // set by CombatArmyCreator.cs
    public Vector2 areaExtents;

    PackedScene armySoldierScene;
    Node2D armySoldiers;

    // dict of soldiers and their spawn points
    Dictionary<CombatArmySoldier, Vector2> preparedSoldiers = new Dictionary<CombatArmySoldier, Vector2>();

    Tween tween;

    float spawnOffset = 350f; // how many pixels to the right guards spawn in so that they're off-screen initially

    public override void _Ready()
    {
        CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        RectangleShape2D newShape = new RectangleShape2D();
        collisionShape.Shape = newShape;
        newShape.Extents = areaExtents;

        armySoldierScene = GD.Load<PackedScene>("res://src/combat/army/CombatArmySoldier.tscn");
        armySoldiers = GetNode<Node2D>("ArmySoldiers");

        tween = GetNode<Tween>("Tween");
    }

    async Task<bool> IsSoliderColliding(CombatArmySoldier soldier)
    {
        await ToSignal(GetTree().CreateTimer(0.0375f, false), "timeout");
        return soldier.GetOverlappingAreas().Count >= 1;
    }

    void PlaceSoldierRandomPos(CombatArmySoldier soldier)
    {
        // the CollisionShape is at (0, 0); so half of it is to the left (of this.position) and half to the right (of this.position) 
        // we want to spawn a solider somewhere within this area, so we get a random range between this area
        GD.Randomize();
        float newSoldierPosX = (float)GD.RandRange(-(areaExtents.x / 2), (areaExtents.x / 2));
        float newSoldierPosY = (float)GD.RandRange(-(areaExtents.y / 2), (areaExtents.y / 2));

        Vector2 spawnPos = new Vector2(newSoldierPosX, newSoldierPosY);

        soldier.Position = spawnPos;
    }

    // figure out the placement of the solider
    // called by CombatArmyCreator during the round switch when the vfx are happening
    async public Task PrepArmySoldier(int soliderNum, Enums.ArmyGunTypes gunType)
    {
        CombatArmySoldier newSoldier = armySoldierScene.Instance<CombatArmySoldier>();
        newSoldier.gunType = gunType;

        // hide the soldier and keep trying random positions till it isnt colliding with any of the existing soldiers
        newSoldier.Visible = false;
        armySoldiers.AddChild(newSoldier);

        do 
        {
            PlaceSoldierRandomPos(newSoldier);
        }
        while (await IsSoliderColliding(newSoldier));

        newSoldier.Visible = true;

        // re-calculate the spawn and off-screen spawn positions 
        // also set the soldier's position to the off-screen value; prevously it was the final position to check for collisions
        Vector2 spawnPos = newSoldier.Position;
        Vector2 offScreenSpawnPos = new Vector2(newSoldier.Position.x + spawnOffset, newSoldier.Position.y);
        newSoldier.Position = offScreenSpawnPos;

        preparedSoldiers.Add(newSoldier, spawnPos);
    }

    // animate entrance of prepared soldiers
    // called by CombatArmyCreator when the new round starts
    async public void DeployArmySoldier()
    {
        float animLength = 1.5f;

        foreach (KeyValuePair<CombatArmySoldier, Vector2> kvp in preparedSoldiers)
        {
            CombatArmySoldier newSoldier = kvp.Key;
            Vector2 spawnPos = kvp.Value;

            // animate the soldier coming in from the right side of the screen
            tween.InterpolateProperty(newSoldier, "position", null, spawnPos, animLength);
            tween.Start();
            newSoldier.animPlayer.Play("move_" + newSoldier.gunType.ToString().ToLower());

            await ToSignal(newSoldier.animPlayer, "animation_finished");

            // set the idle animation of their gun
            newSoldier.animPlayer.AssignedAnimation = newSoldier.animString;
            newSoldier.animPlayer.Seek(0f, true);
        }
    }
}
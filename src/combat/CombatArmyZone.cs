using Godot;

public class CombatArmyZone : Area2D
{
    // set by CombatArmyCreator.cs
    public Vector2 areaExtents;

    PackedScene armySoldierScene;
    Node2D armySoldiers;

    Tween tween;

    public override void _Ready()
    {
        CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        RectangleShape2D newShape = new RectangleShape2D();
        collisionShape.Shape = newShape;
        newShape.Extents = areaExtents; 

        armySoldierScene = GD.Load<PackedScene>("res://src/actors/CombatArmySoldier.tscn");
        armySoldiers = GetNode<Node2D>("ArmySoldiers");

        tween = GetNode<Tween>("Tween");
    }

    async public void SummonArmySoldier(int soliderNum, Enums.ArmyGunTypes gunType)
    {
        // the CollisionShape is at (0, 0); so half of it is to the left (of this.position) and half to the right (of this.position) 
        // we want to spawn a solider somewhere within this area, so we get a random range between this area
        GD.Randomize();
        float newSoldierPosX = (float) GD.RandRange(-(areaExtents.x / 2), (areaExtents.x / 2));
        float newSoldierPosY = (float) GD.RandRange(-(areaExtents.y / 2), (areaExtents.y / 2));

        CombatArmySoldier newSoldier = armySoldierScene.Instance<CombatArmySoldier>();

        Vector2 offScreenSpawnPos = new Vector2(newSoldierPosX + 350f, newSoldierPosY);
        Vector2 spawnPos = new Vector2(newSoldierPosX, newSoldierPosY);
        newSoldier.Position = offScreenSpawnPos;
        newSoldier.gunType = gunType;

        armySoldiers.AddChild(newSoldier);

        float animLength = 2f;
        
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
using Godot;

public class CombatArmyZone : Area2D
{
    // set by CombatArmyCreator.cs
    public Vector2 areaExtents;

    PackedScene armySoliderScene;
    Node2D armySoliders;

    public override void _Ready()
    {
        CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        RectangleShape2D newShape = new RectangleShape2D();
        collisionShape.Shape = newShape;
        newShape.Extents = areaExtents;

        armySoliderScene = GD.Load<PackedScene>("res://src/actors/CombatArmySoldier.tscn");
        armySoliders = GetNode<Node2D>("ArmySoldiers");
    }

    public void SummonArmySolider(int soliderNum, Enums.ArmyGunTypes gunType)
    {
        // the CollisionShape is at (0, 0); so half of it is to the left (of this.position) and half to the right (of this.position) 
        // we want to spawn a solider somewhere within this area, so we get a random range between this area
        float newSoldierPosX = (float) GD.RandRange(-(areaExtents.x / 2), (areaExtents.x / 2));
        float newSoldierPosY = (float) GD.RandRange(-(areaExtents.y / 2), (areaExtents.y / 2));

        CombatArmySoldier newSoldier = armySoliderScene.Instance<CombatArmySoldier>();
        newSoldier.Position = new Vector2(newSoldierPosX, newSoldierPosY);
        newSoldier.gunType = gunType;

        armySoliders.AddChild(newSoldier);

        // TODO: have a cool animation for the solider walking in from the right side of the screen
    }

}

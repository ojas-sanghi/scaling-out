using System.Collections.Generic;
using Godot;
using Godot.Collections;

public class GuardFOV : Node2D
{
    [Export] float fieldOfView = 50;
    [Export] float radiusWarn = 350;
    [Export] float radiusDanger = 350;

    [Export] bool showCircle = false;
    [Export] bool showFov = true;
    [Export] bool showTargetLine = false;

    [Export] Color circleColor = new Color("9f185c0b");

    [Export] Color fovColor = new Color("b2ff0000");
    [Export] Color fovWarnColor = new Color("ffff00");
    [Export] Color fovDangerColor = new Color("ff0000");

    [Export] float viewDetail = 60;

    [Export] Array<string> enemyGroups = new() { "scientist" };

    public Array<Godot.Object> inWarnArea = new();
    public Array<Godot.Object> inDangerArea = new();

    record ArcPoint(Vector2 pos, int level);

    // Buffer to target points
    List<ArcPoint> pointsArc = new();
    bool isUpdate = false;


    public override void _Ready()
    {
        var directionDeg = Mathf.Rad2Deg(Transform.Rotation);
        var startAngle = (directionDeg - (fieldOfView / 2)) + 45; // + 90
        var endAngle = startAngle + fieldOfView;

        var startVector = Position + DegToVector(startAngle) * radiusWarn;
        var endVector = Position + DegToVector(endAngle) * radiusWarn;

        LightOccluder2D occluder = GetNode<LightOccluder2D>("LightOccluder2D");

        if (GetParent().Name == "SecurityCamera")
        {
            return;
        }
        
        CollisionShape2D collisionShape2D = GetParent().GetNode<CollisionShape2D>("CollisionShape2D");
        Vector2 shapeExtents = ((RectangleShape2D)collisionShape2D.Shape).Extents;

        occluder.Occluder.Polygon = new Vector2[] { new Vector2(5, 0), startVector, new Vector2(-shapeExtents.x, -shapeExtents.y), new Vector2(-shapeExtents.x, shapeExtents.y), endVector};

        // TODO
        // detect and send signal when the soldier EXITS the warn area
        // and then make the color go back to white form yellow

        // figure out same lighting for the camera --> right now we just return if its the camera :D
        
        // and then, the rest of the implemention for the camera as professed in gitkraken boards
        // AND implemention for soldier as profressed in my notes app :)

        //? better infra for stealth enemies -- maybe BaseStealthEnemy that has exported fov that the actual fov thing uses to calculate the vision? and also the light stuff uses to make the occluders and everything?? not sure. but the infra right now is bad so need to make it better
    }


    public override void _Process(float delta)
    {
        isUpdate = true;
        CheckView();
        Update();
    }

    public override void _Draw()
    {
        if (showCircle)
        {
            DrawCircle(Position, radiusWarn, circleColor);
        }

        if (showFov && isUpdate)
        {
            DrawCircleArc();
        }
    }

    void DrawCircleArc()
    {
        foreach (ArcPoint aux in pointsArc)
        {
            if (aux == null) continue;

            if (aux.level == 1 && showTargetLine)
            {
                DrawLine(Position, aux.pos, fovWarnColor);
            }
            else if (aux.level == 2 && showTargetLine)
            {
                DrawLine(Position, aux.pos, fovDangerColor, 3);
            }
            else
            {
                DrawLine(Position, aux.pos, fovColor);
            }
        }
    }

    Vector2 DegToVector(float deg)
    {
        return new Vector2(
            Mathf.Cos(Mathf.Deg2Rad(deg)),
            Mathf.Sin(Mathf.Deg2Rad(deg))
        );
    }

    void CheckView()
    {
        var directionDeg = Mathf.Rad2Deg(Transform.Rotation);
        var startAngle = directionDeg - (fieldOfView / 2);
        var endAngle = startAngle + fieldOfView;

        pointsArc = new();
        inDangerArea = new();
        inWarnArea = new();

        var spaceState = GetWorld2d().DirectSpaceState;

        for (int i = 0; i < viewDetail + 1; i++)
        {
            var currentAngle = startAngle + (i * (fieldOfView / viewDetail)) + 90;
            Vector2 point = Position + DegToVector(currentAngle) * radiusWarn;

            // use global coordinates, not local to node
            Dictionary result = spaceState.IntersectRay(
                GetGlobalTransform().origin, ToGlobal(point), new Array() { GetParent() }
            );

            if (result.Count > 0)
            {
                Vector2 resultPosition = (Vector2)result["position"];
                Node2D resultCollider = (Node2D)result["collider"];
                var localPos = ToLocal(resultPosition);
                var dist = Position.DistanceTo(localPos);
                int level = 0;
                bool isEnemy = false;

                if (inDangerArea.Contains(resultCollider) || inWarnArea.Contains(resultCollider))
                {
                    pointsArc.Add(new ArcPoint(localPos, level));
                }
                else
                {
                    foreach (var g in enemyGroups)
                    {
                        if (resultCollider.GetGroups().Contains(g))
                        {
                            isEnemy = true;
                        }
                    }

                    if (isEnemy)
                    {
                        level = 1;
                        if (dist < radiusDanger)
                        {
                            level = 2;
                            inDangerArea.Add(resultCollider);

                            Events.publishLevelFailed();
                        }
                        else
                        {
                            inWarnArea.Add(resultCollider);
                            Events.publishScientistEnteredWarnZone();
                        }
                        // check if directly to target, we can "shoot"
                        var tgtPos = resultCollider.GetGlobalTransform().origin;
                        var thisPos = GetGlobalTransform().origin;
                        var tgtDir = (tgtPos - thisPos).Normalized();
                        var viewAngle = Mathf.Rad2Deg
                        (
                            DegToVector(
                                Mathf.Rad2Deg(
                                    GetGlobalTransform().Rotation
                                ) + 90
                            ).AngleTo(tgtDir)
                        );

                        if (viewAngle > startAngle && viewAngle < endAngle)
                        {
                            Dictionary result2 = spaceState.IntersectRay(thisPos, tgtPos, new Array() { GetParent() });
                            Node2D result2Collider = (Node2D)result2["collider"];

                            if (result2 != null && result2Collider == resultCollider)
                            {
                                // we can then use this as line
                                pointsArc.Add(new ArcPoint(localPos, 0));
                                if (showTargetLine)
                                {
                                    pointsArc.Add(new ArcPoint(ToLocal(localPos), level));
                                }
                            }
                            else
                            {
                                pointsArc.Add(new ArcPoint(localPos, level));
                            }
                        }
                        else
                        {
                            pointsArc.Add(new ArcPoint(localPos, level));
                        }
                    }
                    else
                    {
                        pointsArc.Add(new ArcPoint(localPos, level));
                    }
                }
            }
            else
            {
                pointsArc.Add(new ArcPoint(point, 0));
            }
        }
    }
}
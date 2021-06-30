using System.Collections.Generic;
using Godot;

public class Lane : Path2D
{
    [Export] public Texture laneImg;
    [Export] float curveSmoothFactor;

    public List<BaseDino> dangerDinos = new List<BaseDino>();
    public bool inDanger = false;

    Vector2 spawnPoint;
    List<PathFollow2D> newChildren = new List<PathFollow2D>();

    public override void _Ready()
    {
        if (laneImg == null)
        {
            GD.PushError("You must set laneImg for Lane!");
            GD.PrintStack();
            GetTree().Quit(1);
        }

        TextureButton button = GetNode<TextureButton>("Button");
        spawnPoint = button.RectGlobalPosition + new Vector2(70, 30);

        GetNode<Sprite>("Sprite").Texture = laneImg;
        Events.newRound += OnNewRound;
        Events.dinoDiedInstance += OnDinoDiedInstance;

        // we're an empty lane, don't allow deploys
        if (Curve.GetPointCount() == 0)
        {
            button.Hide();
            return;
        }

        // make a hitbox for the danger zone
        var dangerBox = new RectangleShape2D();
        // divide by 1/2 since that's how extents work
        // divide x by 1/3 since we want it to be that big
        dangerBox.Extents = new Vector2(laneImg.GetSize().x / 2 / 3, laneImg.GetSize().y / 2 * GetNode<Sprite>("Sprite").Scale.y);
        var dangerZoneCollision = GetNode("DangerZone").GetNode<CollisionShape2D>("DangerZoneCollision");
        dangerZoneCollision.Shape = dangerBox;
        dangerZoneCollision.Position = new Vector2(this.GlobalPosition.x / 1.255f, 0);
    }

    public override void _ExitTree()
    {
        Events.newRound -= OnNewRound;
    }


    // sort in order of progress on the lane
    public class sortDinos : IComparer<PathFollow2D>
    {
        public int Compare(PathFollow2D a, PathFollow2D b)
        {
            if (a.UnitOffset < b.UnitOffset)
            {
                return 1;
            }
            if (a.UnitOffset > b.UnitOffset)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    List<PathFollow2D> GetSortedPositionDinos()
    {
        List<PathFollow2D> sortedChildren = new List<PathFollow2D>(newChildren);
        sortedChildren.Sort(new sortDinos());
        return sortedChildren;
    }

    void OnButtonPressed()
    {
        // dont deploy if the delay isn't over yet
        if (CombatInfo.Instance.dinosDeploying.Contains(CombatInfo.Instance.selectedDinoType))
        {
            return;
        }

        // dont deploy if we can't afford it
        if (!DinoInfo.Instance.CanAffordDino(CombatInfo.Instance.selectedDinoType))
        {
            return;
        }

        // deduct dino cost
        CombatInfo.Instance.creds -= DinoInfo.Instance.GetDinoDeployCost(CombatInfo.Instance.selectedDinoType);

        // make a new pathfollow
        PathFollow2D pathFollow = new PathFollow2D();
        pathFollow.Loop = false;
        pathFollow.Lookahead = curveSmoothFactor;
        AddChild(pathFollow);
        newChildren.Add(pathFollow);

        // make a new dino
        PackedScene dinoScene = DinoInfo.Instance.dinoList[CombatInfo.Instance.selectedDinoType];
        BaseDino dinoNode = (BaseDino)dinoScene.Instance();

        // set dino speed
        dinoNode.pathFollowTime = Curve.GetBakedLength() / dinoNode.dinoSpeed.x;

        // add dino to pathfollow
        pathFollow.AddChild(dinoNode);
        // set dinos position
        dinoNode.GlobalPosition = spawnPoint;

        Events.publishDinoDeployed(dinoNode.dinoType);
    }

    void OnDangerZoneAreaEntered(Area2D area)
    {
        BaseDino dinoEntered = (BaseDino)area;
        inDanger = true;

        if (!dangerDinos.Contains(dinoEntered))
            dangerDinos.Add(dinoEntered);
        
        if (!CombatInfo.Instance.lanesInDanger.Contains(this))
            CombatInfo.Instance.lanesInDanger.Add(this);

    }

    void OnDinoDiedInstance(BaseDino dino)
    {
        if (dangerDinos.Contains(dino))
            dangerDinos.Remove(dino);
        
        if (dangerDinos.Count == 0)
        {
            inDanger = false;
            CombatInfo.Instance.lanesInDanger.Remove(this);
        }
    }

    void OnNewRound()
    {
        foreach (PathFollow2D child in newChildren)
        {
            child.QueueFree();
        }
        newChildren.Clear();
    }

    void OnTimerTimeout()
    {
        GetSortedPositionDinos();
    }

}

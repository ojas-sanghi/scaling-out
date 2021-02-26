using System.Collections.Generic;
using Godot;

public class Lane : Path2D
{
    [Export] Texture laneImg;
    [Export] float curveSmoothFactor;

    Vector2 spawnPoint = new Vector2(70, 10);
    List<PathFollow2D> newChildren = new List<PathFollow2D>();

    public override void _Ready()
    {
        GetNode<Sprite>("Sprite").Texture = laneImg;
        Events.newRound += OnNewRound;
    }

    public class sortDinos : IComparer<PathFollow2D>
    {
        public int Compare(PathFollow2D a, PathFollow2D b)
        {
            if (a.UnitOffset > b.UnitOffset)
            {
                return 1;
            }
            if (a.UnitOffset < b.UnitOffset)
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
        //? Test if this works
        sortedChildren.Sort(new sortDinos());
        return sortedChildren;
    }

    //? does this engine signal connection work
    void _on_Button_pressed()
    {
        if (CombatInfo.Instance.dinosRemaining <= 0)
        {
            return;
        }

        // dont deploy if the delay isn't over yet
        if (CombatInfo.Instance.dinosDeploying.Contains(CombatInfo.Instance.dinoId))
        {
            return;
        }

        // make a new pathfollow
        PathFollow2D pathFollow = new PathFollow2D();
        pathFollow.Loop = false;
        pathFollow.Lookahead = curveSmoothFactor;
        AddChild(pathFollow);
        newChildren.Add(pathFollow);

        // make a new dino
        PackedScene dinoScene = DinoInfo.Instance.dinoList[CombatInfo.Instance.dinoId];
        BaseDino dinoNode = (BaseDino)dinoScene.Instance();

        // set dino speed
        dinoNode.pathFollowTime = Curve.GetBakedLength() / dinoNode.dinoSpeed.x;

        // add dino to pathfollow
        pathFollow.AddChild(dinoNode);
        // set dinos position
        dinoNode.Position = spawnPoint;
        Events.publishDinoDeployed();
    }

    void OnNewRound()
    {
        foreach (PathFollow2D child in newChildren)
        {
            child.QueueFree();
        }
        newChildren.Clear();
    }

    //? does this engine signal connection work
    void _on_Timer_timeout()
    {
        OnNewRound();
    }

}
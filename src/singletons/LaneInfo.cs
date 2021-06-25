using System.Collections.Generic;
using Godot;

public class LaneInfo : Node
{
    public static LaneInfo Instance;
    
    public Dictionary<Enums.LaneTypes, LaneDetails> laneInfoList;
    public record LaneDetails(Texture image, Curve2D curve);

    public LaneInfo()
    {
        Instance = this;
    }
    
    public override void _Ready()
    {
        Instance = this;
        laneInfoList = new Dictionary<Enums.LaneTypes, LaneDetails>()
            {
                {Enums.LaneTypes.RoadStraight, new LaneDetails
                    (
                        GD.Load<Texture>("res://assets/combat/lanes/road-straight.png"),
                        GD.Load<Curve2D>("res://src/combat/lanes/curves/RoadStraight.tres")
                    )
                },

                {Enums.LaneTypes.RoadEmpty, new LaneDetails
                    (
                        GD.Load<Texture>("res://assets/combat/lanes/road-empty.png"),
                        new Curve2D()
                    )
                }

            };
    }


}
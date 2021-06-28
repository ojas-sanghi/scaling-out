using Godot;
using Godot.Collections;

public class LanesCreator : Node2D
{
    Dictionary<int, double> yPositionForNumLanes = new Dictionary<int, double>()
    {
        {1, 532},
        {2, 327},
        {3, 258},
        {4, 224},
        {5, 203}
    };

    // programmatically makes lanes at startup based on a config for how many lanes the specific conquest needs
    public override void _Ready()
    {
        var laneTypes = CitiesInfo.Instance.currentCity.lanesInfo;

        // set number of lanes and position of the parent of the lanes (this node) based off dict
        // hard coded, but no other way
        int numLanes = laneTypes.Count;
        this.Position = new Vector2((float)700.5, (float)yPositionForNumLanes[numLanes]);

        // Calculate how much x and y space we have in total
        ColorRect TopBar = (ColorRect)GetParent().FindNode("TopBar");
        ColorRect BottomBar = (ColorRect)GetParent().FindNode("BottomBar");
        ColorRect ArmyBase = (ColorRect)GetParent().FindNode("ArmyBase");
        double lanesXAvailable = GetViewportRect().Size.x - ArmyBase.RectSize.x;
        double lanesYAvailable = GetViewportRect().Size.y - TopBar.RectSize.y - BottomBar.RectSize.y;

        // Figure out how much y space for each alne
        double yPixelsForEachLane = lanesYAvailable / numLanes;

        // info about which lane in the list we're on -- used to set position of the lane later
        int laneIndex = 0;
        foreach (Enums.LaneTypes type in laneTypes)
        {
            // get their info
            var laneDetails = LaneInfo.Instance.laneInfoList[type];

            // make new lane and set some info
            Lane newLane = GD.Load<PackedScene>("res://src/combat/lanes/Lane.tscn").Instance<Lane>();
            newLane.laneImg = laneDetails.image;
            newLane.Curve = laneDetails.curve;
            newLane.Position = new Vector2(0, (float)yPixelsForEachLane * laneIndex);

            // calculate scale of the lane texture
            double imgXScale = lanesXAvailable / laneDetails.image.GetWidth();
            double imgYScale = yPixelsForEachLane / laneDetails.image.GetHeight();
            Vector2 newScaleVector = new Vector2((float)imgXScale, (float)imgYScale);

            // get sprite and set scale
            Sprite newLaneSprite = newLane.GetNode<Sprite>("Sprite");
            newLaneSprite.Scale = newScaleVector;

            // get button and set the position (using the scale, its weird)
            TextureButton button = newLane.GetNode<TextureButton>("Button");
            button.RectPosition *= newScaleVector;

            AddChild(newLane);

            laneIndex++;
        }
    }

}

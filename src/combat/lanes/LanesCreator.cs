using Godot;
using Godot.Collections;

public class LanesCreator : Node2D
{
    Dictionary<int, float> yPositionForNumLanes = new Dictionary<int, float>()
    {
        {1, 532f},
        {2, 327f},
        {3, 258f},
        {4, 224f},
        {5, 203f}
    };

    // exposed because CombatArmyCreator uses it too
    public float yPixelsForEachLane = 0;
    public int numLanes = 1;

    // programmatically makes lanes at startup based on a config for how many lanes the specific conquest needs
    public override void _Ready()
    {
        var laneTypes = CityInfo.Instance.currentCity.lanesInfo;

        // set the number of lanes and position of the parent of the lanes (this node) based off dict
        // it's hard coded, but no other way :/
        numLanes = laneTypes.Count;
        this.Position = new Vector2(700.5f, yPositionForNumLanes[numLanes]);

        // Calculate how much x and y space we have in total
        ColorRect TopBar = (ColorRect)GetParent().FindNode("TopBar");
        ColorRect BottomBar = (ColorRect)GetParent().FindNode("BottomBar");
        ColorRect ArmyBase = (ColorRect)GetParent().FindNode("ArmyBase");
        float lanesXAvailable = GetViewportRect().Size.x - ArmyBase.RectSize.x;
        float lanesYAvailable = GetViewportRect().Size.y - TopBar.RectSize.y - BottomBar.RectSize.y;

        // Figure out how much y space for each lane
        yPixelsForEachLane = lanesYAvailable / numLanes;

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
            newLane.Position = new Vector2(0, yPixelsForEachLane * laneIndex);

            // calculate scale of the lane texture
            float imgXScale = lanesXAvailable / laneDetails.image.GetWidth();
            float imgYScale = yPixelsForEachLane / laneDetails.image.GetHeight();
            Vector2 newScaleVector = new Vector2(imgXScale, imgYScale);

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
